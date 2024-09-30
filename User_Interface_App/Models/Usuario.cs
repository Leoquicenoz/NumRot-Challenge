namespace User_Interface_App.Models
{
    public class Usuario
    {
        public required string NumeroDocumento { get; set; }
        public required string PrimerNombre { get; set; }
        public string? SegundoNombre { get; set; }  // Opcional
        public required string PrimerApellido { get; set; }
        public string? SegundoApellido { get; set; }  // Opcional
        public string? Telefono { get; set; }  // Opcional
        public string? CorreoElectronico { get; set; }  // Opcional
        public required string Direccion { get; set; }
        public required int Edad { get; set; }
        public required string Genero { get; set; }
    }
}