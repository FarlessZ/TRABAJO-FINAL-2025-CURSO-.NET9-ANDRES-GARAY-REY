using BlazorBootstrap;
using Microsoft.AspNetCore.Components;

namespace PortalGalaxy.WebApp.Pages.Reportes;

public partial class ReporteTalleresPorInstructor
{
    private BarChart _barChart = null!;
    private ChartData _chartData = null!;
    private BarChartOptions _chartOptions = null!;

    private List<int> ListAnios { get; set; } = new();

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await DataInicialAsync();
        }

        await base.OnAfterRenderAsync(firstRender);
    }

    private async Task DataInicialAsync()
    {
        _chartData = new ChartData
        {
            Labels = new List<string>
            {
                "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre",
                "Noviembre", "Diciembre"
            },
            Datasets = new List<IChartDataset>()
            {
                new BarChartDataset()
                {
                    Label = "Cantidad de Talleres",
                    Data = Enumerable.Range(0, 3).Select(_ => (double?)0).ToList(),
                    BackgroundColor = new List<string> { "#000455"},
                    CategoryPercentage = 0.9,
                    BarPercentage = 1
                }
            }
        };


        _chartOptions = new BarChartOptions();

        _chartOptions.Interaction.Mode = InteractionMode.Index;
        _chartOptions.Responsive = true;
        _chartOptions.IndexAxis = "y";

        if (_chartOptions.Scales.X is { Title: not null })
        {
            _chartOptions.Scales.X.Title.Text = "Mes";
            _chartOptions.Scales.X.Title.Display = true;
        }

        if (_chartOptions.Scales.Y is { Title: not null })
        {
            _chartOptions.Scales.Y.Title.Text = "Cantidad";
            _chartOptions.Scales.Y.Title.Display = true;
        }

        await _barChart.InitializeAsync(_chartData, _chartOptions);
    }

    protected override void OnInitialized()
    {
        for (var year = DateTime.Now.Year; year >= 2020; year--)
        {
            ListAnios.Add(year);
        }
    }

    private async Task CargarDatos(int anio)
    {
        try
        {
            var response = await TallerProxy.ListarPorInstructorAsync(anio);
            if (response.Success && response.Data is not null)
            {
                var labels = response.Data.Select(r => r.Instructor).ToList();
                var data = response.Data.Select(r => (double?)r.Cantidad).ToList();

                _chartData.Labels?.Clear();
                _chartData.Labels?.AddRange(labels);

                if (_chartData.Datasets is not null)
                {
                    var firstDataset = (BarChartDataset)_chartData.Datasets[0];
                    firstDataset.Data = data;
                }

                if (_chartOptions.Scales.X is { Title: not null })
                {
                    _chartOptions.Scales.X.Title.Text = "Instructor";
                    _chartOptions.Scales.X.Title.Display = true;
                }

                if (_chartOptions.Scales.Y is { Title: not null })
                {
                    _chartOptions.Scales.Y.Title.Text = "Cantidad";
                    _chartOptions.Scales.Y.Title.Display = true;
                }

                await _barChart.UpdateAsync(_chartData, _chartOptions);

            }

        }
        catch (Exception ex)
        {
            ToastService.ShowError(ex.Message);
        }
    }


    private async Task CambiarAnio(ChangeEventArgs? e)
    {
        if (e is { Value: not null })
        {
            await CargarDatos(int.Parse(e.Value.ToString()!));
        }
    }
}