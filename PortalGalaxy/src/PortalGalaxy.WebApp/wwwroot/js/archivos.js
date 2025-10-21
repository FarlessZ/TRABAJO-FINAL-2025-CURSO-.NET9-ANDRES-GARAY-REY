function descargarArchivo(filename, content, fileType) {
    const file = new File([content], filename, { type: fileType });
    const exportUrl = URL.createObjectURL(file);

    const a = document.createElement("a");
    a.href = exportUrl;
    a.download = filename;
    a.target = "_blank";
    a.click();
    
    URL.revokeObjectURL(exportUrl);
}