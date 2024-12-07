using System;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;

namespace LoginSystem
{
    class Program
    {
        static string connectionString = "Server=localhost\\SQLEXPRESS; Database=Biblioteca; Integrated Security=True;";

        static void Main(string[] args)
        {
            Console.WriteLine("Bienvenido al sistema de gestión de empleados.");
            Console.WriteLine("1. Iniciar sesión");
            Console.WriteLine("2. Crear nuevo empleado");
            Console.Write("Seleccione una opción: ");
            string opcion = Console.ReadLine();

            switch (opcion)
            {
                case "1":
                    IniciarSesion();
                    break;
                case "2":
                    CrearNuevoEmpleado();
                    break;
                default:
                    Console.WriteLine("Opción no válida.");
                    break;
            }
        }

        static void IniciarSesion()
        {
            Console.Write("Ingrese su nombre de usuario: ");
            string nombreUsuario = Console.ReadLine();

            Console.Write("Ingrese su contraseña: ");
            string contrasena = Console.ReadLine();
            string contrasenaHasheada = ObtenerContrasenaHasheada(contrasena);

            // Verifica si las credenciales son correctas
            using (SqlConnection conexion = new SqlConnection(connectionString))
            {
                conexion.Open();
                string query = "SELECT * FROM Empleado WHERE nombre = @nombre AND contraseña = @contrasena";
                SqlCommand comando = new SqlCommand(query, conexion);
                comando.Parameters.AddWithValue("@nombre", nombreUsuario);
                comando.Parameters.AddWithValue("@contrasena", contrasenaHasheada);

                SqlDataReader reader = comando.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();
                    int idEmpleado = (int)reader["id_empleado"]; // Obtener id_empleado del inicio de sesión
                    Console.WriteLine("¡Inicio de sesión exitoso!");
                    MenuEmpleado(idEmpleado);
                }
                else
                {
                    Console.WriteLine("Nombre de usuario o contraseña incorrectos.");
                }
            }
        }

        static void CrearNuevoEmpleado()
        {
            Console.Write("Ingrese el nombre del nuevo empleado: ");
            string nombre = Console.ReadLine();

            Console.Write("Ingrese la contraseña para el nuevo empleado: ");
            string contrasena = Console.ReadLine();
            string contrasenaHasheada = ObtenerContrasenaHasheada(contrasena);

            // Guardar el nuevo empleado en la base de datos
            using (SqlConnection conexion = new SqlConnection(connectionString))
            {
                conexion.Open();
                string query = "INSERT INTO Empleado (nombre, contraseña, estado) VALUES (@nombre, @contrasena, @estado)";
                SqlCommand comando = new SqlCommand(query, conexion);
                comando.Parameters.AddWithValue("@nombre", nombre);
                comando.Parameters.AddWithValue("@contrasena", contrasenaHasheada);
                comando.Parameters.AddWithValue("@estado", "Activo");

                int filasAfectadas = comando.ExecuteNonQuery();
                if (filasAfectadas > 0)
                {
                    Console.WriteLine("Empleado creado exitosamente.");
                }
                else
                {
                    Console.WriteLine("Error al crear el empleado.");
                }
            }

            // Opción para volver al menú de inicio de sesión
            Console.WriteLine("¿Desea iniciar sesión con la nueva cuenta? (S/N): ");
            string respuesta = Console.ReadLine();

            if (respuesta.Equals("S", StringComparison.OrdinalIgnoreCase))
            {
                IniciarSesion(); // Llamar al método de inicio de sesión si el usuario desea intentarlo
            }
            else
            {
                Console.WriteLine("Regresando al menú principal...");
                Main(null); // Vuelve al menú principal
            }
        }

        static string ObtenerContrasenaHasheada(string contrasena)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(contrasena));
                StringBuilder sb = new StringBuilder();
                foreach (byte b in bytes)
                {
                    sb.Append(b.ToString("x2"));
                }
                return sb.ToString();
            }
        }

        // Menú que se presenta después de un inicio de sesión exitoso
        static void MenuEmpleado(int idEmpleado)
        {
            string opcion; // Declaración de la variable opción, fuera del ciclo

            // Ciclo que mantiene el menú activo hasta que el usuario decida salir
            do
            {
                Console.Clear();
                Console.WriteLine("Seleccione una opción:");
                Console.WriteLine("1. Registrar un nuevo usuario");
                Console.WriteLine("2. Ver usuarios");
                Console.WriteLine("3. Actualizar usuario");
                Console.WriteLine("4. Inactivar usuario");
                Console.WriteLine("5. Activar usuario");
                Console.WriteLine("6. Gestionar Préstamos");
                Console.WriteLine("7. Salir");

                opcion = Console.ReadLine(); // Aquí solo asignamos el valor a la variable 'opcion'

                switch (opcion)
                {
                    case "1":
                        RegistrarUsuario(idEmpleado); // Llamada al método para registrar un usuario
                        break;
                    case "2":
                        VerUsuarios(); // Llamada al método para ver los usuarios
                        break;
                    case "3":
                        ActualizarUsuario(); // Llamada al método para actualizar un usuario
                        break;
                    case "4":
                        InactivarUsuario(); // Llamada al método para inactivar un usuario
                        break;
                    case "5":
                        ActivarUsuario(); // Llamada al método para inactivar un usuario
                        break;
                    case "6":
                        GestionarPrestamos(); // Llamada al método para inactivar un usuario
                        break;
                    case "7":
                        Console.WriteLine("Saliendo...");
                        break;
                    default:
                        Console.WriteLine("Opción no válida.");
                        break;
                }

            } while (opcion != "7"); // El ciclo continuará hasta que el usuario elija la opción 5 (Salir)
         
        }
       
        // Registrar un nuevo usuario
        static void RegistrarUsuario(int idEmpleado)
        {
            Console.WriteLine("Ingrese el nombre del nuevo usuario: ");
            string nombre = Console.ReadLine();
            Console.WriteLine("Ingrese el correo del nuevo usuario: ");
            string correo = Console.ReadLine();
            Console.WriteLine("Ingrese el teléfono del nuevo usuario: ");
            string telefono = Console.ReadLine();
            Console.WriteLine("Ingrese la dirección del nuevo usuario: ");
            string direccion = Console.ReadLine();
            Console.WriteLine("Ingrese el tipo de usuario: ");
            string tipoUsuario = Console.ReadLine();

            using (SqlConnection conexion = new SqlConnection(connectionString))
            {
                conexion.Open();
                string query = "INSERT INTO Usuarios (nombre, correo, telefono, direccion, tipo_usuario, id_empleado, created_by, updated_by, created_at, updated_at, estado) " +
                               "VALUES (@nombre, @correo, @telefono, @direccion, @tipo_usuario, @id_empleado, @created_by, @updated_by, @created_at, @updated_at, 'Activo')";
                SqlCommand comando = new SqlCommand(query, conexion);
                comando.Parameters.AddWithValue("@nombre", nombre);
                comando.Parameters.AddWithValue("@correo", correo);
                comando.Parameters.AddWithValue("@telefono", telefono);
                comando.Parameters.AddWithValue("@direccion", direccion);
                comando.Parameters.AddWithValue("@tipo_usuario", tipoUsuario);
                comando.Parameters.AddWithValue("@id_empleado", idEmpleado);
                comando.Parameters.AddWithValue("@created_by", idEmpleado);
                comando.Parameters.AddWithValue("@updated_by", idEmpleado);
                comando.Parameters.AddWithValue("@created_at", DateTime.Now);
                comando.Parameters.AddWithValue("@updated_at", DateTime.Now);

                int filasAfectadas = comando.ExecuteNonQuery();
                if (filasAfectadas > 0)
                {
                    Console.WriteLine("Usuario registrado exitosamente.");
                }
                else
                {
                    Console.WriteLine("Error al registrar el usuario.");
                }
                // Espera a que el usuario presione Enter antes de regresar al menú
                Console.WriteLine("\nPresione Enter para regresar al menú...");
                Console.ReadLine(); // Esto detiene el flujo hasta que el usuario presione Enter
            }
        }

        // Ver los usuarios registrados
        static void VerUsuarios()
        {
            using (SqlConnection conexion = new SqlConnection(connectionString))
            {
                conexion.Open();
                string query = "SELECT id_usuario, nombre, correo, telefono, direccion, tipo_usuario, estado FROM Usuarios WHERE estado = 'Activo'";
                SqlCommand comando = new SqlCommand(query, conexion);
                SqlDataReader reader = comando.ExecuteReader();

                Console.WriteLine("Usuarios Activos:");
                while (reader.Read())
                {
                    Console.WriteLine($"ID: {reader["id_usuario"]}, Nombre: {reader["nombre"]}, Correo: {reader["correo"]}, Teléfono: {reader["telefono"]}, Dirección: {reader["direccion"]}, Tipo: {reader["tipo_usuario"]}, Estado: {reader["estado"]}");
                }
                // Espera a que el usuario presione Enter antes de regresar al menú
                Console.WriteLine("\nPresione Enter para regresar al menú...");
                Console.ReadLine(); // Esto detiene el flujo hasta que el usuario presione Enter
            }
        }

        // Actualizar un usuario
        static void ActualizarUsuario()
        {
            Console.WriteLine("Ingrese el ID del usuario a actualizar:");
            int idUsuario = int.Parse(Console.ReadLine());

            Console.WriteLine("Ingrese el nuevo correo del usuario: ");
            string nuevoCorreo = Console.ReadLine();

            using (SqlConnection conexion = new SqlConnection(connectionString))
            {
                conexion.Open();
                string query = "UPDATE Usuarios SET correo = @correo, updated_at = @updated_at WHERE id_usuario = @id_usuario";
                SqlCommand comando = new SqlCommand(query, conexion);
                comando.Parameters.AddWithValue("@correo", nuevoCorreo);
                comando.Parameters.AddWithValue("@updated_at", DateTime.Now);
                comando.Parameters.AddWithValue("@id_usuario", idUsuario);

                int filasAfectadas = comando.ExecuteNonQuery();
                if (filasAfectadas > 0)
                {
                    Console.WriteLine("Usuario actualizado exitosamente.");
                }
                else
                {
                    Console.WriteLine("Error al actualizar el usuario.");
                }
                // Espera a que el usuario presione Enter antes de regresar al menú
                Console.WriteLine("\nPresione Enter para regresar al menú...");
                Console.ReadLine(); // Esto detiene el flujo hasta que el usuario presione Enter
            }
        }
        static void InactivarUsuario()
        {
            Console.WriteLine("Ingrese el ID del usuario a inactivar:");
            int idUsuario = int.Parse(Console.ReadLine());

            Console.WriteLine("Ingrese su ID de empleado (quién está actualizando):");
            int idEmpleado = int.Parse(Console.ReadLine());  // El ID del empleado que está actualizando

            using (SqlConnection conexion = new SqlConnection(connectionString))
            {
                try
                {
                    conexion.Open();

                    // Primero, verificamos si el usuario existe
                    string checkQuery = "SELECT COUNT(*) FROM Usuarios WHERE id_usuario = @id_usuario AND estado = 'Activo'";
                    SqlCommand checkCommand = new SqlCommand(checkQuery, conexion);
                    checkCommand.Parameters.AddWithValue("@id_usuario", idUsuario);

                    int usuarioActivo = (int)checkCommand.ExecuteScalar();

                    if (usuarioActivo > 0)
                    {
                        // Si el usuario está activo, lo inactivamos
                        string query = "UPDATE Usuarios SET estado = 'Inactivo', updated_by = @updated_by, updated_at = @updated_at WHERE id_usuario = @id_usuario";
                        SqlCommand comando = new SqlCommand(query, conexion);
                        comando.Parameters.AddWithValue("@id_usuario", idUsuario);
                        comando.Parameters.AddWithValue("@updated_by", idEmpleado);  // ID del empleado que realiza la actualización
                        comando.Parameters.AddWithValue("@updated_at", DateTime.Now);  // Fecha de actualización

                        int rowsAffected = comando.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            Console.WriteLine("Usuario inactivado exitosamente.");
                        }
                        else
                        {
                            Console.WriteLine("No se pudo inactivar el usuario.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("El usuario no existe o ya está inactivo.");
                    }
                    // Espera a que el usuario presione Enter antes de regresar al menú
                    Console.WriteLine("\nPresione Enter para regresar al menú...");
                    Console.ReadLine(); // Esto detiene el flujo hasta que el usuario presione Enter
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }

            }
        }
        // Activar un usuario
        static void ActivarUsuario()
        {
            Console.WriteLine("Ingrese el ID del usuario a Activar:");
            int idUsuario = int.Parse(Console.ReadLine());

            Console.WriteLine("Ingrese su ID de empleado (quién está actualizando):");
            int idEmpleado = int.Parse(Console.ReadLine());  // El ID del empleado que está actualizando

            using (SqlConnection conexion = new SqlConnection(connectionString))
            {
                try
                {
                    conexion.Open();

                    // Primero, verificamos si el usuario existe
                    string checkQuery = "SELECT COUNT(*) FROM Usuarios WHERE id_usuario = @id_usuario AND estado = 'Inactivo'";
                    SqlCommand checkCommand = new SqlCommand(checkQuery, conexion);
                    checkCommand.Parameters.AddWithValue("@id_usuario", idUsuario);

                    int usuarioActivo = (int)checkCommand.ExecuteScalar();

                    if (usuarioActivo > 0)
                    {
                        // Si el usuario está Inactivo, lo Activamos
                        string query = "UPDATE Usuarios SET estado = 'Activo', updated_by = @updated_by, updated_at = @updated_at WHERE id_usuario = @id_usuario";
                        SqlCommand comando = new SqlCommand(query, conexion);
                        comando.Parameters.AddWithValue("@id_usuario", idUsuario);
                        comando.Parameters.AddWithValue("@updated_by", idEmpleado);  // ID del empleado que realiza la actualización
                        comando.Parameters.AddWithValue("@updated_at", DateTime.Now);  // Fecha de actualización

                        int rowsAffected = comando.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            Console.WriteLine("Usuario Activar exitosamente.");
                        }
                        else
                        {
                            Console.WriteLine("No se pudo Activar el usuario.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("El usuario no existe o ya está Activo.");
                    }
                    // Espera a que el usuario presione Enter antes de regresar al menú
                    Console.WriteLine("\nPresione Enter para regresar al menú...");
                    Console.ReadLine(); // Esto detiene el flujo hasta que el usuario presione Enter
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }

            }


        }

        // Métodos de gestión de préstamos
        static void GestionarPrestamos()
        {
            Console.WriteLine("1. Realizar nuevo préstamo");
            Console.WriteLine("2. Ver préstamos");
            Console.WriteLine("3. Actualizar préstamo");
            Console.WriteLine("4. Inactivar préstamo");
            Console.Write("Seleccione una opción: ");
            string opcion = Console.ReadLine();

            switch (opcion)
            {
                case "1":
                    RealizarPrestamo();
                    break;
                case "2":
                    VerPrestamos();
                    break;
                case "3":
                    ActualizarPrestamo();
                    break;
                case "4":
                    InactivarPrestamo();
                    break;
                default:
                    Console.WriteLine("Opción no válida.");
                    break;
            }
        }

        static void RealizarPrestamo()
        {
            Console.Write("Ingrese el ID del usuario: ");
            int idUsuario = int.Parse(Console.ReadLine());
            Console.Write("Ingrese el ID del material bibliográfico: ");
            int idMaterial = int.Parse(Console.ReadLine());
            Console.Write("Ingrese la fecha de devolución (YYYY-MM-DD): ");
            DateTime fechaDevolucion = DateTime.Parse(Console.ReadLine());
            DateTime fechaPrestamo = DateTime.Now;

            using (SqlConnection conexion = new SqlConnection(connectionString))
            {
                conexion.Open();
                string query = "INSERT INTO prestamos (fecha_prestamo, fecha_devolucion, id_usuario, id_material, id_empleado, estado) " +
                               "VALUES (@fecha_prestamo, @fecha_devolucion, @id_usuario, @id_material, @id_empleado, 'Activo')";
                SqlCommand comando = new SqlCommand(query, conexion);
                comando.Parameters.AddWithValue("@fecha_prestamo", fechaPrestamo);
                comando.Parameters.AddWithValue("@fecha_devolucion", fechaDevolucion);
                comando.Parameters.AddWithValue("@id_usuario", idUsuario);
                comando.Parameters.AddWithValue("@id_material", idMaterial);
                comando.Parameters.AddWithValue("@id_empleado", 1); // Suponiendo que es un empleado con id=1

                comando.ExecuteNonQuery();
                Console.WriteLine("Préstamo realizado correctamente.");
                // Espera a que el usuario presione Enter antes de regresar al menú
                Console.WriteLine("\nPresione Enter para regresar al menú...");
                Console.ReadLine(); // Esto detiene el flujo hasta que el usuario presione Enter
            }
        }

        static void VerPrestamos()
        {
            Console.Write("Ingrese el ID del usuario: ");
            int idUsuario = int.Parse(Console.ReadLine());

            using (SqlConnection conexion = new SqlConnection(connectionString))
            {
                conexion.Open();
                string query = "SELECT * FROM prestamos WHERE id_usuario = @id_usuario";
                SqlCommand comando = new SqlCommand(query, conexion);
                comando.Parameters.AddWithValue("@id_usuario", idUsuario);

                SqlDataReader reader = comando.ExecuteReader();
                while (reader.Read())
                {
                    Console.WriteLine($"ID Préstamo: {reader["id_prestamo"]}, Fecha Préstamo: {reader["fecha_prestamo"]}, Fecha Devolución: {reader["fecha_devolucion"]}");
                }
                // Espera a que el usuario presione Enter antes de regresar al menú
                Console.WriteLine("\nPresione Enter para regresar al menú...");
                Console.ReadLine(); // Esto detiene el flujo hasta que el usuario presione Enter
            }
        }

        static void ActualizarPrestamo()
        {
            Console.Write("Ingrese el ID del préstamo a actualizar: ");
            int idPrestamo = int.Parse(Console.ReadLine());
            Console.Write("Ingrese la nueva fecha de devolución (YYYY-MM-DD): ");
            DateTime nuevaFechaDevolucion = DateTime.Parse(Console.ReadLine());

            using (SqlConnection conexion = new SqlConnection(connectionString))
            {
                conexion.Open();
                string query = "UPDATE prestamos SET fecha_devolucion = @nuevaFechaDevolucion WHERE id_prestamo = @idPrestamo";
                SqlCommand comando = new SqlCommand(query, conexion);
                comando.Parameters.AddWithValue("@nuevaFechaDevolucion", nuevaFechaDevolucion);
                comando.Parameters.AddWithValue("@idPrestamo", idPrestamo);

                comando.ExecuteNonQuery();
                Console.WriteLine("Préstamo actualizado correctamente.");
            }
            // Espera a que el usuario presione Enter antes de regresar al menú
            Console.WriteLine("\nPresione Enter para regresar al menú...");
            Console.ReadLine(); // Esto detiene el flujo hasta que el usuario presione Enter
        }

        static void InactivarPrestamo()
        {
            Console.Write("Ingrese el ID del préstamo a inactivar: ");
            int idPrestamo = int.Parse(Console.ReadLine());

            // Verificamos si el préstamo existe y si está activo
            using (SqlConnection conexion = new SqlConnection(connectionString))
            {
                conexion.Open();
                // Verificar si el préstamo está activo antes de inactivarlo
                string checkQuery = "SELECT COUNT(*) FROM prestamos WHERE id_prestamo = @idPrestamo AND estado = 1";
                SqlCommand checkCommand = new SqlCommand(checkQuery, conexion);
                checkCommand.Parameters.AddWithValue("@idPrestamo", idPrestamo);

                int prestamoActivo = (int)checkCommand.ExecuteScalar();

                if (prestamoActivo > 0)
                {
                    // Si el préstamo está activo, lo inactivamos
                    string query = "UPDATE prestamos SET estado = 'Inactivo' WHERE id_prestamo = @idPrestamo";
                    SqlCommand comando = new SqlCommand(query, conexion);
                    comando.Parameters.AddWithValue("@idPrestamo", idPrestamo);

                    comando.ExecuteNonQuery();
                    Console.WriteLine("Préstamo inactivado correctamente.");
                }
                else
                {
                    // Si el préstamo no está activo o no existe
                    Console.WriteLine("El préstamo no se encuentra activo o no existe.");
                }
            }

            // Espera a que el usuario presione Enter antes de regresar al menú
            Console.WriteLine("\nPresione Enter para regresar al menú...");
            Console.ReadLine(); // Esto detiene el flujo hasta que el usuario presione Enter
        }

    }
}