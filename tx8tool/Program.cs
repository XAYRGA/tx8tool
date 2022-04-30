using System;
using System.IO;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using xayrga;

internal static class Program
{

	private static void Main(string[] args)
	{

		int opFlag = -1;
		cmdarg.cmdargs = args;
		string operation = cmdarg.assertArg(0, "Operation");
		string inFilePath = null;
		string outFilePath = null;
		string inFileExt = Path.GetExtension(operation)!.ToUpper();

		if (!(inFileExt == ".TX8"))
		{
			if (inFileExt == ".DDS")
			{
				inFilePath = operation;
				opFlag = 2;
				outFilePath = Path.GetFileName(Path.GetDirectoryName(operation) + "/" + Path.GetFileNameWithoutExtension(operation) + ".TX8");
			}
		}
		else
		{
			inFilePath = operation;
			opFlag = 1;
			outFilePath = Path.GetFileName(Path.GetDirectoryName(operation) + "/" + Path.GetFileNameWithoutExtension(operation) + ".DDS");
		}
		if (opFlag == -1)
		{
			switch (operation)
			{
				case "encode":
					inFilePath = cmdarg.assertArg(1, "File Name");
					outFilePath = cmdarg.assertArg(2, "Output File");
					Encode(inFilePath, outFilePath);
					break;
				case "decode":
					inFilePath = cmdarg.assertArg(1, "File Name");
					outFilePath = cmdarg.assertArg(2, "Output File");
					Decode(inFilePath, outFilePath);
					break;
				default:
					Console.WriteLine("tx8tool by Xayrga\ntx8tool.exe <file>\t Converts a .DDS into a TX8 or TX8 into a DDS\n\tencode <file> <output file>\n\tdecode <file> <output file>");
					break;
			}
		}
		else
		{
			switch (opFlag)
			{
				case 1:
					Decode(inFilePath, outFilePath);
					break;
				case 2:
					Encode(inFilePath, outFilePath);
					break;
			}
		}
	}

	private static void Encode(string inF, string outF)
	{
		byte[] bufferFile = File.ReadAllBytes(inF);
		byte[] bufferFullFile = new byte[bufferFile.Length + 0x20];

		for (int i = 0; i < bufferFile.Length; i++)
			bufferFullFile[i] = bufferFile[i];


		var md5 = MD5.Create();
		var bx = md5.ComputeHash(bufferFile);
		var hash = Encoding.ASCII.GetBytes(BitConverter.ToString(bx).Replace("-", ""));

		for (int i = 0; i < hash.Length; i++)
			bufferFullFile[(bufferFile.Length) + i] = hash[i];

		for (int i = 0; i < bufferFullFile.Length; i++)
			unchecked {bufferFullFile[i] -= 96;}
	

		File.WriteAllBytes(outF, bufferFullFile);	
	}

	private static void Decode(string inF, string outF)
	{
		byte[] bufferFile = File.ReadAllBytes(inF);
		byte[] file = new byte[bufferFile.Length - 0x20];
		for (int i = 0; i < bufferFile.Length - 0x20; i++)
		{
			unchecked
			{
				file[i] = (bufferFile[i] += 96);
			}
		}
		File.WriteAllBytes(outF, file);
	}
}
