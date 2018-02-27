using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace tcp
{
	class file_server
	{
		/// <summary>
		/// The PORT
		/// </summary>
		const int PORT = 9000;
		/// <summary>
		/// The BUFSIZE
		/// </summary>
		const int BUFSIZE = 1000;
		IPAddress localAddr = IPAddress.Parse("10.0.0.1");
		byte[] buff = new byte[BUFSIZE];
		/// <summary>
		/// Initializes a new instance of the <see cref="file_server"/> class.
		/// Opretter en socket.
		/// Venter på en connect fra en klient.
		/// Modtager filnavn
		/// Finder filstørrelsen
		/// Kalder metoden sendFile
		/// Lukker socketen og programmet
 		/// </summary>
		private file_server ()
		{

			//opret socket
			TcpListener serverSocket = new TcpListener (localAddr, PORT);
			TcpClient clientSocket = default(TcpClient);
			serverSocket.Start();
			Console.WriteLine("Server started");

			//wait for client
			clientSocket = serverSocket.AcceptTcpClient();
			Console.WriteLine ("client connected");

			//opretter en stream fra client
			NetworkStream serverStreamIO = clientSocket.GetStream(); 
			Console.WriteLine(" >> Accepted connection from client");

			//modtager filnavn
			string fileDir = @"/root/Desktop/Exercise_6_c#/file_server/bin/Debug/files/";
			string userfile = tcp.LIB.readTextTCP (serverStreamIO);

			fileDir += userfile;
			//check for exsitens af fil
			long lengthOfFile = tcp.LIB.check_File_Exists(fileDir);

			if(lengthOfFile != 0)//filen findes
			{
				Console.WriteLine("filen findes " + fileDir);
				//find størrelsen på filen
				long filesize = new System.IO.FileInfo(fileDir).Length;
				Console.WriteLine ("Her" + filesize);
				//send the file
				sendFile (fileDir, filesize, serverStreamIO);
			}
			else //filen exsitere ikke
			{
				Console.WriteLine ("Filen findes ikke " + fileDir);
				tcp.LIB.writeTextTCP (serverStreamIO, "filen findes ikke");
			}

			clientSocket.Close();
			serverSocket.Stop();
		}

		/// <summary>
		/// Sends the file.
		/// </summary>
		/// <param name='fileName'>
		/// The filename.
		/// </param>
		/// <param name='fileSize'>
		/// The filesize.
		/// </param>
		/// <param name='io'>
		/// Network stream for writing to the client.
		/// </param>
		private void sendFile (String fileName, long fileSize, NetworkStream io)
		{


			//send filstørelse

			//send flere gange (10 størrelse) 

			buff = File.ReadAllBytes (fileName);
			//sender filen
			Console.WriteLine ("sended the fileContent: " + System.Text.Encoding.ASCII.GetString(buff));
			io.Write (buff, 0, buff.Length);

			Console.WriteLine (buff);

		}

		/// <summary>
		/// The entry point of the program, where the program control starts and ends.
		/// </summary>
		/// <param name='args'>
		/// The command-line arguments.
		/// </param>
		public static void Main (string[] args)
		{
			Console.WriteLine ("Server starts...");
			new file_server();
		}
	}
}
