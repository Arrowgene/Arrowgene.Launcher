namespace Arrowgene.Launcher.Translation.Languages
{
    public class Spanish : Language
    {
        public Spanish()
        {
            _translations.Add("language_name", "Español");
            _translations.Add("yes", "Sí");
            _translations.Add("no", "No");
            _translations.Add("start", "Comienzo");
            _translations.Add("remember_login", "Recuerda iniciar sesión");
            _translations.Add("file_path", "Ruta de archivo:");
            _translations.Add("client_version", "Versión de cliente:");
            _translations.Add("latest_client_version", "Ultima:");
            _translations.Add("set_game_location", "Establecer la ubicación del juego");
            _translations.Add("download_client", "Descargar cliente");
            _translations.Add("check_for_updates", "Buscar actualizaciones");
            _translations.Add("close", "Cerrar");
            _translations.Add("status", "Estado");
            _translations.Add("title", "Título");
            _translations.Add("message", "Mensaje");
            _translations.Add("change_language", "Cambiar idioma");
            _translations.Add("could_not_select_game", "No se pudo seleccionar el juego");
            _translations.Add("no_games", "No hay juegos disponibles");
            _translations.Add("could_not_find_selected_file", "No se pudo encontrar el archivo seleccionado");
            _translations.Add("new_launcher_download_now", "Una nueva versión de lanzador está disponible, ¿descargarla ahora?");
            _translations.Add("launcher_update", "Actualización del lanzador");
            _translations.Add("select_game_executable", "Selecciona el ejecutable de {0} en la carpeta del juego");  // {0} = Ez2On | Dance!
            _translations.Add("please_restart", "Por favor reinicie la aplicación para que tenga efecto.");
            _translations.Add("notice", "Información");
            _translations.Add("could_not_find_url", "URL del lanzador no se pudo encontrar");
            _translations.Add("unknown_client", "Error en la verificación de la versión del cliente, su cliente no es compatible y no se puede verificar si hay actualizaciones");
            _translations.Add("new_client_download_now", "Nuevo cliente detectado, descargar ahora?");
            _translations.Add("can_not_find_executable", "No se puede encontrar ejecutable. Por favor, establece una ubicación del juego ({0})"); // {0} = C://game/game.exe
            _translations.Add("failed_to_create_args", "Error al crear argumentos de inicio");
            _translations.Add("failed_to_resolve_ip", "No se puede encontrar la dirección IP de '{0}'"); // {0} = https://domain.tld
            _translations.Add("download_from_mirror", "Descargar desde el servidor de archivos? (Más rápido)");
            _translations.Add("after_download_instruction", "Una vez completada la descarga: Extraiga el archivo y use el botón 'Establecer ubicación del juego'");
            _translations.Add("window_mode", "Modo ventana");
        }

        public override string FlagRessource => "pack://application:,,,/es.png";

        public override LanguageType LanguageType => LanguageType.Spanish;
    }
}
