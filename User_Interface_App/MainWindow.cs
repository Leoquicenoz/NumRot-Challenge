using Gtk;
using System;
using System.Text.RegularExpressions;
using System.Data.SQLite;
using User_Interface_App.Models;

namespace User_Interface_App
{
    public class MainWindow : Window
    {
        public MainWindow() : base("Formulario de Usuario")
        {
            SetDefaultSize(500, 600);
            SetPosition(WindowPosition.Center);

            var grid = new Grid
            {
                ColumnSpacing = 10,
                RowSpacing = 10   
            };

            // Crear etiquetas y campos de entrada
            var lblNumeroDocumento = new Label("Número de Documento:");
            var txtNumeroDocumento = new Entry();

            var lblPrimerNombre = new Label("Primer Nombre:");
            var txtPrimerNombre = new Entry();

            var lblSegundoNombre = new Label("Segundo Nombre (Opcional):");
            var txtSegundoNombre = new Entry();

            var lblPrimerApellido = new Label("Primer Apellido:");
            var txtPrimerApellido = new Entry();

            var lblSegundoApellido = new Label("Segundo Apellido (Opcional):");
            var txtSegundoApellido = new Entry();

            var lblTelefono = new Label("Teléfono (Opcional):");
            var txtTelefono = new Entry();

            var lblCorreoElectronico = new Label("Correo Electrónico (Opcional):");
            var txtCorreoElectronico = new Entry();

            var lblDireccion = new Label("Dirección:");
            var txtDireccion = new Entry();

            var lblEdad = new Label("Edad:");
            var txtEdad = new Entry();

            var lblGenero = new Label("Género (M/F):");
            var txtGenero = new Entry();

            var btnGuardar = new Button("Guardar");

            // Evento de guardar datos con validaciones
            btnGuardar.Clicked += (sender, e) =>
            {
                string mensajeValidacion = ValidarFormulario(txtNumeroDocumento, txtPrimerNombre, txtPrimerApellido, txtCorreoElectronico, txtDireccion, txtEdad, txtGenero);

                if (string.IsNullOrEmpty(mensajeValidacion))
                {
                    var usuario = new Usuario
                    {
                        NumeroDocumento = txtNumeroDocumento.Text,
                        PrimerNombre = txtPrimerNombre.Text,
                        SegundoNombre = txtSegundoNombre.Text,
                        PrimerApellido = txtPrimerApellido.Text,
                        SegundoApellido = txtSegundoApellido.Text,
                        Telefono = txtTelefono.Text,
                        CorreoElectronico = txtCorreoElectronico.Text,
                        Direccion = txtDireccion.Text,
                        Edad = int.Parse(txtEdad.Text),
                        Genero = txtGenero.Text
                    };

                    GuardarEnBaseDeDatos(usuario);
                    Console.WriteLine("Datos guardados correctamente.");
                }
                else
                {
                    var dialogo = new MessageDialog(this, DialogFlags.Modal, MessageType.Error, ButtonsType.Ok, mensajeValidacion);
                    dialogo.Run();
                    dialogo.Destroy();
                }
            };  


            var btnConsultar = new Button("Consultar Estadísticas");
            btnConsultar.Clicked += (sender, e) => {
                ConsultarEstadisticas();
            };

            grid.Attach(lblNumeroDocumento, 0, 0, 1, 1);
            grid.Attach(txtNumeroDocumento, 1, 0, 1, 1);
            grid.Attach(lblPrimerNombre, 0, 1, 1, 1);
            grid.Attach(txtPrimerNombre, 1, 1, 1, 1);
            grid.Attach(lblSegundoNombre, 0, 2, 1, 1);
            grid.Attach(txtSegundoNombre, 1, 2, 1, 1);
            grid.Attach(lblPrimerApellido, 0, 3, 1, 1);
            grid.Attach(txtPrimerApellido, 1, 3, 1, 1);
            grid.Attach(lblSegundoApellido, 0, 4, 1, 1);
            grid.Attach(txtSegundoApellido, 1, 4, 1, 1);
            grid.Attach(lblTelefono, 0, 5, 1, 1);
            grid.Attach(txtTelefono, 1, 5, 1, 1);
            grid.Attach(lblCorreoElectronico, 0, 6, 1, 1);
            grid.Attach(txtCorreoElectronico, 1, 6, 1, 1);
            grid.Attach(lblDireccion, 0, 7, 1, 1);
            grid.Attach(txtDireccion, 1, 7, 1, 1);
            grid.Attach(lblEdad, 0, 8, 1, 1);
            grid.Attach(txtEdad, 1, 8, 1, 1);
            grid.Attach(lblGenero, 0, 9, 1, 1);
            grid.Attach(txtGenero, 1, 9, 1, 1);
            grid.Attach(btnGuardar, 0, 10, 2, 1);
            grid.Attach(btnConsultar, 0, 11, 2, 1);

            Add(grid);

            ShowAll();
        }

        // Validar los datos del formulario
        static private string ValidarFormulario(Entry numeroDocumento, Entry primerNombre, Entry primerApellido, Entry correoElectronico, Entry direccion, Entry edad, Entry genero)
        {
            if (string.IsNullOrWhiteSpace(numeroDocumento.Text))
                return "El número de documento es obligatorio.";

            if (string.IsNullOrWhiteSpace(primerNombre.Text))
                return "El primer nombre es obligatorio.";

            if (string.IsNullOrWhiteSpace(primerApellido.Text))
                return "El primer apellido es obligatorio.";

            if (!string.IsNullOrEmpty(correoElectronico.Text) && !EsCorreoValido(correoElectronico.Text))
                return "El correo electrónico no es válido.";

            if (string.IsNullOrWhiteSpace(direccion.Text))
                return "La dirección es obligatoria.";

            if (!int.TryParse(edad.Text, out int edadValida) || edadValida <= 0)
                return "La edad debe ser un número válido.";

            if (string.IsNullOrWhiteSpace(genero.Text) || !(genero.Text.ToUpper() == "M" || genero.Text.ToUpper() == "F"))
                return "El género debe ser 'M' o 'F'.";

            return string.Empty;
        }

        // Validar formato de email
        static private bool EsCorreoValido(string correo)
        {
            string patronCorreo = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(correo, patronCorreo);
        }

        // Guardar los datos en la base de datos
        private static void GuardarEnBaseDeDatos(Usuario usuario)
        {
            using (var conexion = new SQLiteConnection("Data Source=datos_usuario.db"))
            {
                conexion.Open();
                var comando = conexion.CreateCommand();
                comando.CommandText = @"
                    CREATE TABLE IF NOT EXISTS DatosUsuario (
                        NumeroDocumento TEXT PRIMARY KEY,
                        PrimerNombre TEXT,
                        SegundoNombre TEXT,
                        PrimerApellido TEXT,
                        SegundoApellido TEXT,
                        Telefono TEXT,
                        CorreoElectronico TEXT,
                        Direccion TEXT,
                        Edad INTEGER,
                        Genero TEXT
                    );
                    INSERT INTO DatosUsuario (NumeroDocumento, PrimerNombre, SegundoNombre, PrimerApellido, SegundoApellido, Telefono, CorreoElectronico, Direccion, Edad, Genero)
                    VALUES (@numeroDocumento, @primerNombre, @segundoNombre, @primerApellido, @segundoApellido, @telefono, @correoElectronico, @direccion, @edad, @genero);";

                comando.Parameters.AddWithValue("@numeroDocumento", usuario.NumeroDocumento);
                comando.Parameters.AddWithValue("@primerNombre", usuario.PrimerNombre);
                comando.Parameters.AddWithValue("@segundoNombre", usuario.SegundoNombre);
                comando.Parameters.AddWithValue("@primerApellido", usuario.PrimerApellido);
                comando.Parameters.AddWithValue("@segundoApellido", usuario.SegundoApellido);
                comando.Parameters.AddWithValue("@telefono", usuario.Telefono);
                comando.Parameters.AddWithValue("@correoElectronico", usuario.CorreoElectronico);
                comando.Parameters.AddWithValue("@direccion", usuario.Direccion);
                comando.Parameters.AddWithValue("@edad", usuario.Edad);
                comando.Parameters.AddWithValue("@genero", usuario.Genero);

                comando.ExecuteNonQuery();
            }

            Console.WriteLine("Datos guardados en la base de datos.");
        }


        // Realizar consultas y estadísticas
        private void ConsultarEstadisticas()
        {
            using (var conexion = new SQLiteConnection("Data Source=datos_usuario.db"))
            {
                conexion.Open();

                // Primero verificamos cuántos registros hay en la base de datos
                var contarRegistros = conexion.CreateCommand();
                contarRegistros.CommandText = "SELECT COUNT(*) FROM DatosUsuario;";
                int totalRegistros = Convert.ToInt32(contarRegistros.ExecuteScalar());

                if (totalRegistros < 10)
                {
                    int faltan = 10 - totalRegistros;
                    var dialogo = new MessageDialog(this, DialogFlags.Modal, MessageType.Info, ButtonsType.Ok, 
                        $"Faltan {faltan} registro(s) para llegar a 10.");
                    dialogo.Run();
                    dialogo.Destroy();
                    return;
                }

                // Consulta de nombres completos
                var consultaNombres = conexion.CreateCommand();
                consultaNombres.CommandText = "SELECT PrimerNombre || ' ' || PrimerApellido AS NombreCompleto FROM DatosUsuario;";
                var nombresReader = consultaNombres.ExecuteReader();
                Console.WriteLine("Nombres completos de las personas insertadas:");
                while (nombresReader.Read())
                {
                    Console.WriteLine(nombresReader["NombreCompleto"]);
                }
                nombresReader.Close();

                // Contar mujeres
                var contarMujeres = conexion.CreateCommand();
                contarMujeres.CommandText = "SELECT COUNT(*) FROM DatosUsuario WHERE Genero = 'F';";
                int totalMujeres = Convert.ToInt32(contarMujeres.ExecuteScalar());
                Console.WriteLine($"Número de mujeres: {totalMujeres}");

                // Contar hombres
                var contarHombres = conexion.CreateCommand();
                contarHombres.CommandText = "SELECT COUNT(*) FROM DatosUsuario WHERE Genero = 'M';";
                int totalHombres = Convert.ToInt32(contarHombres.ExecuteScalar());
                Console.WriteLine($"Número de hombres: {totalHombres}");

                // Persona con mayor edad
                var personaMayor = conexion.CreateCommand();
                personaMayor.CommandText = "SELECT PrimerNombre || ' ' || PrimerApellido AS NombreCompleto FROM DatosUsuario ORDER BY Edad DESC LIMIT 1;";
                var nombreMayor = personaMayor.ExecuteScalar()?.ToString();
                Console.WriteLine($"Persona con mayor edad: {nombreMayor}");

                // Promedio de edad
                var promedioEdad = conexion.CreateCommand();
                promedioEdad.CommandText = "SELECT AVG(Edad) FROM DatosUsuario;";
                double promedio = Convert.ToDouble(promedioEdad.ExecuteScalar());
                Console.WriteLine($"Promedio de edad: {promedio}");
            }
        }
    }
}
