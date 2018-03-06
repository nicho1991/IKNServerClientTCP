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
			Int64 fileSize = 0;
			//make byte array
			Byte[] buff = new byte[BUFSIZE];

			//tell what file we want
			tcp.LIB.writeTextTCP (io, fileName);
		

			//modtag fil størrelse
			fileSize = Int64.Parse( tcp.LIB.readTextTCP(io));
			Console.WriteLine ("size is " + fileSize);


			//modtag fil
			FileStream fs = new FileStream(fileName,FileMode.OpenOrCreate,FileAccess.Write);

			Int32 bytesReceived = 0;
			Int64 totalbytedReceived = 0;
			Int64 megaByte = 1048576;
			while ((bytesReceived = io.Read (buff,0,buff.Length))>0) {
				totalbytedReceived += bytesReceived;
				fs.Write (buff, 0, bytesReceived);
				int percentCompleted = (int)Math.Round(((double)(totalbytedReceived/(double)fileSize)*100));
				Console.Write("\r{0} ", "Received: " + totalbytedReceived/megaByte + " Mbytes" + " Out of " + fileSize/megaByte + " Mbytes" + " total: " +  percentCompleted + " %");
			}
			if (totalbytedReceived > 0) {
				Console.WriteLine("You have received a file! congratulations!");
			}
			else{
				Console.WriteLine ("Sadly you did not receive a file :( ");
			}



			io.Close ();
			fs.Close ();
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
