using QuestPDF.Fluent;
using QuestPDF.Helpers;

namespace PortalGalaxy.Services.Utils;

public static class Helper
{
    public static int GetTotalPages(int totalRows, int pageSize)
    {
        if (totalRows == 0) return 0;

        var total = totalRows / pageSize;
        if (totalRows % pageSize > 0)
            total++;
        return total;
    }
    
    public static void TextData(this TextSpanDescriptor text)
    {
        text.FontFamily("Arial").FontSize(8).FontColor(Colors.Black);
    }
}
