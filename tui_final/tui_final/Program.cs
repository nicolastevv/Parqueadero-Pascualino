using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;

namespace ParqueaderoApp
{
	class Cliente
	{
		public string Nombre { get; set; }
		public string Apellidos { get; set; }
		public string Cedula { get; set; }
		public string NumeroTelefono { get; set; }
		public string CorreoElectronico { get; set; }
		public string TipoVehiculo { get; set; }
		public string Marca { get; set; }
		public string Modelo { get; set; }
		public string Color { get; set; }
		public string Placa { get; set; }
		public string TipoTarifa { get; set; }
		public string HoraEntrada { get; set; }
		public string Sucursal { get; set; }
	}

	static class ValidadorDatos
	{
		public static string ValidarNombreCompleto(string prompt)
		{
			while (true)
			{
				Console.Write(prompt);
				string nombre = Console.ReadLine().Trim();
				string[] partes = Regex.Replace(nombre, " +", " ").Split(' ');

				if (partes.Length >= 2 && partes.Length <= 4)
				{
					bool valido = true;
					foreach (string parte in partes)
					{
						if (parte.Length > 15 || !Regex.IsMatch(parte, @"^[a-zA-ZáéíóúÁÉÍÓÚñÑ ]+$"))
							valido = false;
					}
					if (valido) return nombre;
				}
				
				MostrarError("Nombre inválido. Debe contener 2-4 partes, cada una con máximo 15 letras.");
			}
		}

		public static string ValidarCelular(string prompt)
		{
			while (true)
			{
				Console.Write(prompt);
				string input = Console.ReadLine();

				if (Regex.IsMatch(input, "^3[0-9]{9}$")) return input;
				MostrarError("Número inválido. Debe tener 10 dígitos comenzando con 3.");
			}
		}

		public static string ValidarEmail(string prompt)
		{
			while (true)
			{
				Console.Write(prompt);
				string email = Console.ReadLine().Trim();
				string patron = @"^([a-zA-Z][a-zA-Z0-9]{0,6}[0-9]{0,2})@(gmail|yahoo|hotmail|pascualbravo)\.(com|co|es|edu\.co|com\.co)$";
				
				if (Regex.IsMatch(email, patron)) return email;
				MostrarError("Correo inválido. Formato: usuario1@dominio.com");
			}
		}

		public static string ValidarCedula(string prompt)
		{
			while (true)
			{
				Console.Write(prompt);
				string input = Console.ReadLine();
				long cedula;
				
				if (long.TryParse(input, out cedula) && cedula >= 10000000) return input;
				MostrarError("Cédula inválida. Debe ser numérica y mayor a 10,000,000.");
			}
		}

		public static string ValidarPlacaMoto(string prompt)
		{
			while (true)
			{
				Console.Write(prompt);
				string placa = Console.ReadLine().Trim().ToUpper();
				
				if (Regex.IsMatch(placa, "^[A-Z]{3}[0-9]{2}[A-Z]$")) return placa;
				MostrarError("Placa inválida. Formato: ABC12D");
			}
		}

		public static string ValidarPlacaCarro(string prompt)
		{
			while (true)
			{
				Console.Write(prompt);
				string placa = Console.ReadLine().Trim().ToUpper();
				
				if (Regex.IsMatch(placa, "^[A-Z]{3}[0-9]{3}$")) return placa;
				MostrarError("Placa inválida. Formato: ABC123");
			}
		}

		public static string ValidarHora(string prompt)
		{
			while (true)
			{
				Console.Write(prompt);
				string hora = Console.ReadLine().Trim();

				try
				{
					DateTime.ParseExact(hora, "HH:mm:ss", null);
					return hora;
				}
				catch
				{
					MostrarError("Hora inválida. Formato: HH:MM:SS");
				}
			}
		}

		public static string ValidarModelo(string prompt)
		{
			while (true)
			{
				Console.Write(prompt);
				string textoModelo = Console.ReadLine();
				int modelo;

				if (int.TryParse(textoModelo, out modelo))
				{
					if (modelo >= 2000 && modelo <= DateTime.Now.Year + 1) return textoModelo;
				}
				
				MostrarError(string.Format("Modelo inválido. Debe ser entre 2000 y {0}.", DateTime.Now.Year + 1));
			}
		}

		public static string ValidarTipoVehiculo(string prompt)
		{
			while (true)
			{
				Console.Write(prompt);
				string tipo = Console.ReadLine().ToLower();
				
				if (tipo == "carro" || tipo == "moto")
					return tipo.First().ToString().ToUpper() + tipo.Substring(1);
				
				MostrarError("Tipo inválido. Debe ser 'Carro' o 'Moto'.");
			}
		}

		public static void MostrarError(string mensaje)
		{
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine(mensaje);
			Console.ResetColor();
		}
	}

	class ProgramaParqueadero
	{
		static List<Cliente> clientes = new List<Cliente>();
		const int MAX_CAPACIDAD = 10;

		static void Main(string[] args)
		{
			ConfigureDisplay();
			AboutApp();
			Start();
			Menu();
		}

		static void ConfigureDisplay()
		{
			Console.SetWindowSize(130, 30);
			Console.Title = "Parqueadero Pascualino";
			Console.CursorVisible = true;
		}

		static void AboutApp()
		{
			Console.ForegroundColor = ConsoleColor.Blue;
			Console.WriteLine("░█▀▀█ ─█▀▀█ ░█▀▀█ ░█▀▀█ ░█─░█ ░█▀▀▀ ─█▀▀█ ░█▀▀▄ ░█▀▀▀ ░█▀▀█ ░█▀▀▀█");
			Console.WriteLine("░█▄▄█ ░█▄▄█ ░█▄▄▀ ░█─░█ ░█─░█ ░█▀▀▀ ░█▄▄█ ░█─░█ ░█▀▀▀ ░█▄▄▀ ░█──░█");
			Console.WriteLine("░█─── ░█─░█ ░█─░█ ─▀▀█▄ ─▀▄▄▀ ░█▄▄▄ ░█─░█ ░█▄▄▀ ░█▄▄▄ ░█─░█ ░█▄▄▄█");
			Console.ForegroundColor = ConsoleColor.Gray;
			Console.WriteLine("Parqueadero Pascualino - Versión 2.0");
			Console.WriteLine("Desarrollado por Nicolas Steven Palacios");
			Console.ResetColor();
			//Console.WriteLine();
		}

		static void Start()
		{
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine("*******************************************************************************************************");
			Console.Write("*");
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.Write("[C: Crear Cliente] | [R: Leer Cliente] | [U: Actualizar Cliente] | [D: Eliminar Cliente] | [E: Salir]");
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine("*");
			Console.Write("*******************************************************************************************************");
			Console.ResetColor();
			Console.WriteLine();
		}

		static void Menu()
		{
			bool salir = false;
			while (!salir)
			{
				ConsoleKeyInfo tecla = Console.ReadKey(true);
				switch (tecla.Key)
				{
					case ConsoleKey.C:
						Console.Clear();
						CrearCliente();
						Start();
						break;
					case ConsoleKey.R:
						Console.Clear();
						LeerCliente();
						Start();
						break;
					case ConsoleKey.U:
						Console.Clear();
						ActualizarCliente();
						Start();
						break;
					case ConsoleKey.D:
						Console.Clear();
						EliminarCliente();
						Start();
						break;
					case ConsoleKey.E:
						salir = true;
						ExitApp();
						break;
					default:
						Console.ForegroundColor = ConsoleColor.Red;
						Console.WriteLine("Tecla no válida. Presiona una opción del menú.");
						Console.ResetColor();
						System.Threading.Thread.Sleep(1000);
						Console.Clear();
						AboutApp();
						Start();
						break;
				}
			}
		}
		static string SeleccionarSucursal()
		{
			
			Console.WriteLine("\nSeleccione la sucursal:");
			Console.WriteLine("E - Sucursal Envigado");
			Console.WriteLine("P - Sucursal Poblado");
			while (true)
			{
				Console.Write("Ingrese la letra de la sucursal: ");
				char letra = Char.ToUpper(Console.ReadKey(true).KeyChar);
				Console.WriteLine("");

				if (letra == 'E')
					return "Sucursal Envigado";
				else if (letra == 'P')
					return "Sucursal Poblado";
				else
					Console.WriteLine("Opción inválida. Por favor ingrese E o P.");
			}
		}
			static void CrearCliente()
			{
				Console.Write("--- Creación de Nuevo Cliente ---");
				SeleccionarSucursal();
				Console.Clear();
				

				if (clientes.Count >= MAX_CAPACIDAD)
				{
					ValidadorDatos.MostrarError(string.Format("Capacidad máxima ({0}) alcanzada.", MAX_CAPACIDAD));
					Console.WriteLine("Pulse una tecla para continuar.");
					//Console.ReadKey();
					return;
				}

				Cliente nuevoCliente = new Cliente
				{
					Nombre = ValidadorDatos.ValidarNombreCompleto("Nombre completo (2-4 partes, 15 caracteres max c/u): "),
					Cedula = ValidadorDatos.ValidarCedula("Cédula (mayor a 10,000,000): "),
					NumeroTelefono = ValidadorDatos.ValidarCelular("Celular (10 dígitos empezando con 3): "),
					CorreoElectronico = ValidadorDatos.ValidarEmail("Correo (formato usuario@dominio.com): "),
					TipoVehiculo = ValidadorDatos.ValidarTipoVehiculo("Tipo de vehículo (Carro/Moto): "),
					Marca = ValidadorDatos.ValidarNombreCompleto("Marca del vehículo: "),
					Modelo = ValidadorDatos.ValidarModelo(string.Format("Modelo (2000-{0}): ", DateTime.Now.Year + 1)),
					Color = ValidadorDatos.ValidarNombreCompleto("Color del vehículo: "),
					Placa = ValidarPlacaUnica(),
					TipoTarifa = ValidarTipoTarifa(),
					HoraEntrada = ValidadorDatos.ValidarHora("Hora de entrada (HH:MM:SS): ")
				};

				clientes.Add(nuevoCliente);
				MostrarExito("Cliente creado exitosamente!");
				
			}

			static string ValidarPlacaUnica()
			{
				while (true)
				{
					string placa = "";
					if (clientes.Count > 0 && clientes[clientes.Count - 1].TipoVehiculo == "Moto")
					{
						placa = ValidadorDatos.ValidarPlacaMoto("Placa de moto (ABC12D): ");
					}
					else
					{
						placa = ValidadorDatos.ValidarPlacaCarro("Placa de carro (ABC123): ");
					}

					if (!clientes.Any(c => c.Placa != null && c.Placa.ToUpper() == placa.ToUpper())) return placa;
					ValidadorDatos.MostrarError("Placa ya registrada. Ingrese una nueva.");
				}
			}

			static string ValidarTipoTarifa()
			{
				while (true)
				{
					Console.Write("Tipo de tarifa (Día/Hora): ");
					string tipo = Console.ReadLine().Trim().ToLower();
					
					if (tipo == "día" || tipo == "dia")
						return "Día (10,000 COP)";
					else if (tipo == "hora")
						return "Hora (2,000 COP)";
					
					ValidadorDatos.MostrarError("Opción inválida. Use 'Día' o 'Hora'");
				}
			}

			static void LeerCliente()
			{
				Console.WriteLine("--- Buscar Cliente ---");
				SeleccionarSucursal();
				if (clientes.Count == 0)
				{
					ValidadorDatos.MostrarError("No hay clientes registrados.");
					Console.WriteLine("Presione una tecla para volver al menú...");
					Console.ReadKey(intercept: true);
					return;
				}

				string placa = ValidadorDatos.ValidarPlacaCarro("Ingrese placa a buscar: ");
				Cliente cliente = clientes.FirstOrDefault(c => c.Placa != null && c.Placa.ToUpper() == placa.ToUpper());

				if (cliente != null)
				{
					MostrarInfoCliente(cliente);
					Console.WriteLine("Presione una tecla para volver al menú...");
					Console.ReadKey(intercept: true);
				}
				else
				{
					ValidadorDatos.MostrarError("Cliente no encontrado.");
					Console.WriteLine("Presione una tecla para volver al menú...");
					Console.ReadKey(intercept: true);
				}
				//Console.ReadKey();
			}

			static void ActualizarCliente()
			{
				Console.WriteLine("--- Actualizar Cliente ---");
				SeleccionarSucursal();
				if (clientes.Count == 0)
				{
					ValidadorDatos.MostrarError("No hay clientes registrados.");
					Console.WriteLine("Presione una tecla para volver al menú...");
					Console.ReadKey(intercept: true);
					return;
				}

				string placa = ValidadorDatos.ValidarPlacaCarro("Ingrese placa del cliente: ");
				Cliente cliente = clientes.FirstOrDefault(c => c.Placa != null && c.Placa.ToUpper() == placa.ToUpper());

				if (cliente == null)
				{
					ValidadorDatos.MostrarError("Cliente no encontrado.");
					Console.ReadKey();
					return;
				}

				MostrarMenuActualizacion(cliente);
				MostrarExito("Actualización completada!");
				Console.ReadKey();
			}

			static void MostrarMenuActualizacion(Cliente cliente)
			{
				char opcion;
				do
				{
					Console.Clear();
					MostrarInfoCliente(cliente);
					Console.ForegroundColor = ConsoleColor.Cyan;
					Console.WriteLine("\nSeleccione campo a actualizar:");
					Console.WriteLine("[1] Nombre      [4] Teléfono   [7] Modelo");
					Console.WriteLine("[2] Cédula      [5] Correo     [8] Color");
					Console.WriteLine("[3] Tipo Veh.   [6] Marca      [9] Placa");
					Console.WriteLine("[T] Tarifa      [H] Hora Ent.  [S] Salir");
					Console.ResetColor();

					opcion = char.ToUpper(Console.ReadKey(true).KeyChar);
					
					switch (opcion)
					{
						case '1':
							cliente.Nombre = ValidadorDatos.ValidarNombreCompleto("Nuevo nombre: ");
							break;
						case '2':
							cliente.Cedula = ValidadorDatos.ValidarCedula("Nueva cédula: ");
							break;
						case '3':
							cliente.TipoVehiculo = ValidadorDatos.ValidarTipoVehiculo("Nuevo tipo: ");
							break;
						case '4':
							cliente.NumeroTelefono = ValidadorDatos.ValidarCelular("Nuevo teléfono: ");
							break;
						case '5':
							cliente.CorreoElectronico = ValidadorDatos.ValidarEmail("Nuevo correo: ");
							break;
						case '6':
							cliente.Marca = ValidadorDatos.ValidarNombreCompleto("Nueva marca: ");
							break;
						case '7':
							cliente.Modelo = ValidadorDatos.ValidarModelo("Nuevo modelo: ");
							break;
						case '8':
							cliente.Color = ValidadorDatos.ValidarNombreCompleto("Nuevo color: ");
							break;
						case '9':
							cliente.Placa = ValidarPlacaUnica();
							break;
						case 'T':
							cliente.TipoTarifa = ValidarTipoTarifa();
							break;
						case 'H':
							cliente.HoraEntrada = ValidadorDatos.ValidarHora("Nueva hora: ");
							break;
					}
				} while (opcion != 'S');
			}

			static void EliminarCliente()
			{
				Console.WriteLine("--- Eliminar Cliente ---");
				SeleccionarSucursal();
				if (clientes.Count == 0)
				{
					ValidadorDatos.MostrarError("No hay clientes registrados.");
					Console.WriteLine("Presione una tecla para volver al menú...");
					Console.ReadKey(intercept: true);
					return;
				}

				string placa = ValidadorDatos.ValidarPlacaCarro("Ingrese placa del cliente: ");
				Cliente cliente = clientes.FirstOrDefault(c => c.Placa != null && c.Placa.ToUpper() == placa.ToUpper());

				if (cliente != null)
				{
					clientes.Remove(cliente);
					MostrarExito("Cliente eliminado exitosamente!");
				}
				else
				{
					ValidadorDatos.MostrarError("Cliente no encontrado.");
				}
				//Console.ReadKey();
			}

			static void MostrarInfoCliente(Cliente cliente)
			{
				Console.ForegroundColor = ConsoleColor.Green;
				Console.WriteLine("--- Información del Cliente ---");
				Console.WriteLine("Nombre: {0}", cliente.Nombre);
				Console.WriteLine("Cédula: {0}", cliente.Cedula);
				Console.WriteLine("Teléfono: {0}", cliente.NumeroTelefono);
				Console.WriteLine("Correo: {0}", cliente.CorreoElectronico);
				Console.WriteLine("Vehículo: {0} {1} {2}", cliente.TipoVehiculo, cliente.Marca, cliente.Modelo);
				Console.WriteLine("Color: {0}", cliente.Color);
				Console.WriteLine("Placa: {0}", cliente.Placa);
				Console.WriteLine("Tarifa: {0}", cliente.TipoTarifa);
				Console.WriteLine("Hora Entrada: {0}", cliente.HoraEntrada);
				Console.ResetColor();
			}

			static void MostrarExito(string mensaje)
			{
				Console.ForegroundColor = ConsoleColor.Green;
				Console.WriteLine(mensaje);
				Console.ResetColor();
			}

			static void ExitApp()
			{
				Console.Clear();
				Console.ForegroundColor = ConsoleColor.Blue;
				Console.WriteLine("Gracias por usar Parqueadero Pascualino!");
				Console.WriteLine("Espere por favor 2 segundos");
				Thread.Sleep(2000);
				Environment.Exit(1);
			}
		}
	}