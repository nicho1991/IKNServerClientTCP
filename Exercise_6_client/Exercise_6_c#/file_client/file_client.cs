using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace tcp
{
	class file_client
	{
		/// <summary>
		/// The PORT.
		/// </summary>
		const int PORT = 9000;
		/// <summary>
		/// The BUFSIZE.
		/// </summary>
		const int BUFSIZE = 1000;
		//make the buffer
		byte[] buff = new byte[BUFSIZE];

		/// <summary>
		/// Initializes a new instance of the <see cref="file_client"/> class.
		/// </summary>
		/// <param name='args'>
		/// The command-line arguments. First ip-adress of the server. Second the filename
		/// </param>
		private file_client (string[] args)
		{

			System.Net.Sockets.TcpClient clientSocket = new System.Net.Sockets.TcpClient ();
			//opretter sockets
			Console.WriteLine("client made");
		
			//connecting
			clientSocket.Connect(args[0].ToString(),PORT);
			Console.WriteLine("connected to server");

			//find networkstream
			NetworkStream serverstreamIO = clientSocket.GetStream ();
			Console.WriteLine ("made networkstream");
			receiveFile (args[1].ToString(), serverstreamIO);

		}

		/// <summary>
		/// Receives the file.
		/// </summary>
		/// <param name='fileName'>
		/// File name.
		/// </param>
		/// <param name='io'>
		/// Network stream for reading from the server
		/// </param>
		private void receiveFile (String fileName, NetworkStream io)
		{
			io.Flush ();
			Console.WriteLine ("started recevicing file");
			buff = System.Text.Encoding.ASCII.GetBytes (fileName + "\0");

			//tell server what file we want!
			io.Write (buff, 0, buff.Length);




			string text = System.Text.Encoding.ASCII.GetString(buff);
			

			Console.WriteLine ("sended the request " + text);
			//modtag fil størrelse

			//modtag efter fil størrelse (10)
		
			int size = io.Read (buff, 0, buff.Length);
			Console.WriteLine ("filesize is: " + size);
			Console.WriteLine (System.Text.Encoding.ASCII.GetString(buff));
		}

		/// <summary>
		/// The entry point of the program, where the program control starts and ends.
		/// </summary>
		/// <param name='args'>
		/// The command-line arguments.
		/// </param>
		public static void Main (string[] args)
		{
			Console.WriteLine ("Client starts...");
			new file_client(args);
		}
	}
}
