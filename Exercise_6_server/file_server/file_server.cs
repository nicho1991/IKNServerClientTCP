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
			while (true) {
				//wait for client
				clientSocket = serverSocket.AcceptTcpClient ();
				Console.WriteLine ("client connected");

				//opretter en stream fra client
				NetworkStream serverStreamIO = clientSocket.GetStream (); 
				Console.WriteLine (" >> Accepted connection from client");

				//modtager filnavn
				string fileDir = @"/root/Desktop/IKNServerClientTCP/Exercise_6_server/file_server/bin/Debug/files/";
				string userfile = tcp.LIB.readTextTCP (serverStreamIO);

				fileDir += userfile;
				//check for exsitens af fil
				long lengthOfFile = tcp.LIB.check_File_Exists (fileDir);

				if (lengthOfFile != 0) {//filen findes
					Console.WriteLine ("filen findes " + fileDir);
					//find størrelsen på filen
					//long filesize = new System.IO.FileInfo (fileDir).Length;
					long filesize = LIB.check_File_Exists(fileDir);
					//send the file
					sendFile (fileDir, filesize, serverStreamIO);
				} else { //filen exsitere ikke
					Console.WriteLine ("Filen findes ikke " + fileDir);
					tcp.LIB.writeTextTCP (serverStreamIO, "filen findes ikke");
				}
				//clientSocket.Close ();
			}

			
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
			tcp.LIB.writeTextTCP(io,fileSize.ToString());

			FileStream fs = new FileStream (fileName, FileMode.Open, FileAccess.Read);

			int numberOfPackages = Convert.ToInt32 (Math.Ceiling (Convert.ToDouble (fileSize) / Convert.ToDouble (BUFSIZE)));
			long currentPacketLength = 0;
			long totalLength = fileSize;

			//write out
			for (int i = 0; i < numberOfPackages; i++) {
				if (totalLength > BUFSIZE) {
					currentPacketLength = BUFSIZE;
					totalLength -= BUFSIZE;
	

				} else {

					currentPacketLength = totalLength;
				}

				byte[] sendingBuffer = new byte[currentPacketLength];

				fs.Read (sendingBuffer, 0, (int)currentPacketLength);
				io.Write (sendingBuffer, 0, sendingBuffer.Length);
			}

			fs.Close ();
			io.Close ();
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
