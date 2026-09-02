# Sistema de Reservas de Aulas

Aplicación de escritorio WPF (.NET 10) con acceso a datos ADO.NET puro (sin ORM) para gestionar la reserva de aulas: login contra base de datos, listados en modo conectado y desconectado, búsquedas y registro de nuevas reservas con validación de disponibilidad.

Proyecto académico del curso DAEA — patrón *MVVM* manual (⁠ ViewModelBase ⁠ + ⁠ RelayCommand ⁠, sin frameworks externos como Prism o CommunityToolkit.Mvvm).

## Tecnologías

•⁠  ⁠.NET 10 (⁠ net10.0-windows ⁠), WPF
•⁠  ⁠⁠ Microsoft.Data.SqlClient ⁠ 7.0.2
•⁠  ⁠SQL Server (probado con SQL Server Express, ⁠ localhost\SQLEXPRESS ⁠)
•⁠  ⁠C# 13

## Estructura del proyecto


daea_lab03/
├── Models/          Usuario, Aula, Reserva (POCOs)
├── Data/            Repositorios ADO.NET (SqlConnection/SqlCommand/SqlDataAdapter)
├── ViewModels/       Un ViewModel por vista, hereda de ViewModelBase
├── Views/           UserControls (una vista por caso de uso)
├── MVVM/            ViewModelBase (INotifyPropertyChanged) y RelayCommand (ICommand)
├── LoginWindow.xaml Ventana de ingreso (punto de arranque de la app)
└── MainWindow.xaml  Ventana principal: barra de marca + menú + dashboard de accesos


## Base de datos

El script [⁠ ReservasDB.sql ⁠](ReservasDB.sql) crea la base ⁠ ReservasDB ⁠ con:

•⁠  ⁠⁠ Usuarios(UsuarioId, Username, Password, NombreCompleto) ⁠ — 10 registros de prueba
•⁠  ⁠⁠ Aulas(AulaId, Nombre, Capacidad) ⁠ — 15 registros de prueba
•⁠  ⁠⁠ Reservas(ReservaId, AulaId FK, UsuarioId FK, Fecha, Hora, Motivo) ⁠ — 30 registros de prueba

Ejecutarlo una vez contra tu instancia de SQL Server antes de correr la app:


sqlcmd -S localhost\SQLEXPRESS -i ReservasDB.sql


La cadena de conexión vive en ⁠ daea_lab03/App.config ⁠ bajo la clave ⁠ ReservasDB ⁠. Ajustala si tu servidor/instancia es distinto:

⁠ xml
<add name="ReservasDB"
     connectionString="Server=localhost\SQLEXPRESS;Database=ReservasDB;Trusted_Connection=True;..." />
 ⁠

Usuario de prueba para el login: ⁠ jperez ⁠ / ⁠ jperez123 ⁠ (ver el resto en ⁠ ReservasDB.sql ⁠).

## Cómo correr


dotnet build daea_lab03/daea_lab03.csproj
dotnet run --project daea_lab03


La app arranca en ⁠ LoginWindow ⁠. Al autenticar correctamente abre ⁠ MainWindow ⁠ con el menú principal.

## Funcionalidad

### Login (conectado)

⁠ LoginWindow ⁠ pide usuario y contraseña. ⁠ UsuarioRepository.ValidarCredenciales ⁠ abre una ⁠ SqlConnection ⁠, ejecuta un ⁠ SELECT ⁠ parametrizado contra ⁠ Usuarios ⁠ y lee el resultado con ⁠ SqlDataReader ⁠ mientras la conexión permanece abierta. Si no hay coincidencia se muestra un mensaje de error en la propia ventana; si la hay, se abre ⁠ MainWindow ⁠ pasando el usuario autenticado.

### Aulas

•⁠  ⁠*Aulas (DataTable) — desconectado*: ⁠ AulaRepository.ObtenerTabla() ⁠ usa ⁠ SqlDataAdapter.Fill ⁠, que abre y cierra la conexión internamente. El ⁠ DataGrid ⁠ se llena a partir de ese ⁠ DataTable ⁠ sin conexión activa.
•⁠  ⁠*Aulas (Objetos) — conectado*: ⁠ AulaRepository.ObtenerLista() ⁠ abre la conexión explícitamente y recorre un ⁠ SqlDataReader ⁠ fila por fila construyendo cada ⁠ Aula ⁠.
•⁠  ⁠*Buscar por nombre*: cada clic en "Buscar" ejecuta una consulta nueva (⁠ SqlCommand ⁠ + ⁠ LIKE ⁠ parametrizado) vía ⁠ AulaRepository.BuscarPorNombre ⁠.

### Reservas

Mismo esquema conectado/desconectado que Aulas, con ⁠ ReservaRepository ⁠. Los listados incluyen el nombre del aula y del usuario mediante ⁠ JOIN ⁠ para que la grilla sea legible (no solo IDs).

•⁠  ⁠*Reservas (DataTable)* — desconectado.
•⁠  ⁠*Reservas (Objetos)* — conectado.
•⁠  ⁠*Buscar por fecha* — nueva consulta por cada búsqueda.
•⁠  ⁠*Nueva Reserva* — formulario con aula (ComboBox), fecha (DatePicker) y hora (ComboBox de franjas de 30 min, no texto libre). ⁠ ReservaRepository.Registrar ⁠ verifica en la misma conexión si ya existe una reserva con la misma Aula+Fecha+Hora antes del ⁠ INSERT ⁠; si existe, lanza una excepción que el ViewModel captura y muestra como mensaje, sin insertar.

### Menú principal

⁠ MainWindow ⁠ expone las 5 opciones (Aulas DataTable/Objetos, Reservas DataTable/Objetos, Nueva Reserva) por dos caminos equivalentes: un ⁠ Menu ⁠ clásico y un dashboard de tarjetas — ambos llaman a los mismos métodos, cada uno abre la vista correspondiente en una ventana nueva.

## Arquitectura MVVM

•⁠  ⁠*Model*: ⁠ Usuario ⁠, ⁠ Aula ⁠, ⁠ Reserva ⁠ — clases planas, sin lógica.
•⁠  ⁠*View*: ⁠ UserControl ⁠ por caso de uso. El code-behind solo instancia su ViewModel (⁠ DataContext = new XyzViewModel() ⁠); no contiene lógica de negocio ni acceso a datos.
•⁠  ⁠*ViewModel*: expone propiedades bindeables (⁠ INotifyPropertyChanged ⁠ vía ⁠ ViewModelBase.SetField ⁠) y comandos (⁠ RelayCommand ⁠). Llama a los repositorios, nunca a ADO.NET directamente.
•⁠  ⁠*Data*: un repositorio por entidad, con un método por operación (⁠ ObtenerTabla ⁠, ⁠ ObtenerLista ⁠, ⁠ BuscarPor... ⁠, ⁠ Registrar ⁠, ⁠ ValidarCredenciales ⁠). Es la única capa que conoce ⁠ SqlConnection ⁠/⁠ SqlCommand ⁠.

Excepciones deliberadas al MVVM estricto (documentadas, no accidentales):
•⁠  ⁠⁠ LoginWindow.xaml.cs ⁠ lee ⁠ PasswordBox.Password ⁠ en el evento ⁠ Click ⁠ porque ⁠ PasswordBox ⁠ no expone esa propiedad como bindeable (limitación de seguridad de WPF).
•⁠  ⁠⁠ MainWindow.xaml.cs ⁠ abre las ventanas hijas (⁠ new Window { Content = ... } ⁠) porque es responsabilidad de navegación/presentación, no de negocio.