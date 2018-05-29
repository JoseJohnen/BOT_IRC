﻿using System.Net.Sockets; // Lo mismo que la anterior
using System.IO; // Lo usamos para el manejo con los streams
using System.Text.RegularExpressions; // No es vital pero me encantan el uso de las expresiones regulares
using System;
using System.Threading.Tasks;

namespace BOT_IRC
{
    public class Bot
    {
        public string[] usuarios; // Creamos el string[] usuarios para tener todos los nicks que estan en el canal

        NetworkStream conexion; // Establecemos la variable conexion como NetworkStream
        TcpClient irc; // Establecemos la variable irc como TcpClient
        string code = ""; // Creamos la variable string que vamos a usar para leer los sockets

        public StreamReader leer_datos; // Establecemos la variable leer_datos como StreamReader
        public StreamWriter mandar_datos; // Establecemos la variable mandar_datos como SteamWriter

        string host = "chat.freenode.net"; // Establecemos la variable string host para tener el host del canal IRC
        public string nickname = "ClapTrakaLaKa"; // Establecemos la variable nickname con el nick del bot
        public string canal = "#locos"; // Establecemos la variable canal con el nombre del canal


        public Bot(string host, string nickname, string canal)
        {
            irc = new TcpClient(host, 6667); // Realizamos la conexion con el canal usando el host y el puerto 6667
            conexion = irc.GetStream(); // Cargamos la conexion para poder leer los datos
            leer_datos = new StreamReader(conexion); // Lo necesario para leer los datos de la conexion 
            mandar_datos = new StreamWriter(conexion); // Lo necesario para mandar comandos al canal IRC

            this.host = host;
            this.nickname = nickname;
            this.canal = canal;
        }

        public void PrepareBot()
        {
            this.mandar_datos.WriteLine("NICK " + this.nickname); // Usamos el comando NICK para entrar al canal usando el nick antes declarado
            this.mandar_datos.Flush(); // Actualizamos la conexion

            this.mandar_datos.WriteLine("USER " + this.nickname + " 1 1 1 1"); // Usamos el comando USER para confirmar el nickname
            this.mandar_datos.Flush(); // ..

            this.mandar_datos.WriteLine("JOIN " + this.canal); // Usamos el comando JOIN para entrar al canal
            this.mandar_datos.Flush(); // ..
        }

        public void WorkingBot()
        {
            this.PrepareBot();
            while (true) // Mi bucle enterno
            {
                while ((code = this.leer_datos.ReadLine()) != null) // Leemos la conexion con la variable code
                {
                    Console.WriteLine("Code : " + code); // No es necesario pero es para ver las respuestas del servidor

                    Match regex = Regex.Match(code, "PING(.*)", RegexOptions.IgnoreCase); // Detectamos el clasico PING para el PING PONG
                                                                                          // que nos hara el servidor IRC para verificar que estemos ahi y entrar al canal , aunque lo sigo haciendo despues
                                                                                          // para ver que no estemos muerto o algo asi xD
                    if (regex.Success) // Si se encontro algo
                    {
                        string te_doy_pong = "PONG " + regex.Groups[1].Value; // Capturamos lo que esta despues del ping y le damos al pong con los datos
                        this.mandar_datos.WriteLine(te_doy_pong); // Mandamos el comando de la variable anterior
                        this.mandar_datos.Flush(); // ..
                    }

                    regex = Regex.Match(code, ":(.*) 353 (.*) = (.*) :(.*)", RegexOptions.IgnoreCase); // Capturamos los usuarios de todo el canal
                                                                                                       // con el poder de las expresiones regulares
                    if (regex.Success) // Si los encontraron
                    {
                        string usuarios_lista = regex.Groups[4].Value; // Tenemos la variable con todos los nicks
                        this.usuarios = usuarios_lista.Split(' '); // Para mayor comodidad usamos un split para separar todos los espacios vacios que estan entre
                                                                   // cada nick del canal para despues hacer una lista , que es la primera que declare en el codigo
                        foreach (string usuario in this.usuarios) // Usamos un for each para leer la lista usuarios y mostrar cada nick en la variable usuario
                        {
                            Console.WriteLine("[+] User : " + usuario); // Mostramos cada user
                        }
                    }

                    regex = Regex.Match(code, ":(.*)!(.*) PRIVMSG (.*) :(.*)", RegexOptions.IgnoreCase); // Lo usamos para detectar los mensajes privados y publicos
                    if (regex.Success) // Si se encontro algo
                    {

                        this.mandar_datos.WriteLine("PRIVMSG" + " " + this.canal + " " + ":Hi World"); // Mandamos un mensaje al canal 
                        this.mandar_datos.Flush(); // ..

                        string dedonde = regex.Groups[1].Value; // Se detecta la procedencia del mensaje
                        string mensaje = regex.Groups[4].Value; // El mensaje en si
                        if (dedonde != this.canal) // Si la procedencia del mensaje no es el canal en si activamos esta condicion , cabe aclarar que si es el canal
                                                   // el que nos mando el mensaje es un mensaje PUBLICO , caso contrario es PRIVADO
                        {
                            Console.WriteLine("[+] " + dedonde + " dice : " + mensaje); // Mostramos el dueño del mensaje y el mensaje
                            Match regex_ordenes = Regex.Match(mensaje, "!spam (.*) (.*)", RegexOptions.IgnoreCase); // Esta es la orden !spam con los (.*)
                                                                                                                    // detectamos los dos comandos que son <nick> <mensaje>
                            if (regex_ordenes.Success) // Si se encontro algo
                            {
                                this.mandar_datos.WriteLine("PRIVMSG" + " " + regex_ordenes.Groups[1].Value + " " + regex_ordenes.Groups[2].Value); // Mandamos
                                                                                                                                                    // un mensaje al usuario especificado con el mensaje que pedimos
                                this.mandar_datos.Flush(); // ..
                            }
                        }
                    }
                } //END While Interior
            } //END While Eterno
        }


    }
}