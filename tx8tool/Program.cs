using System;
using System.IO;
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
		for (int i = 0; i < bufferFile.Length; i++)
		{
			unchecked
			{
				bufferFile[i] += 0xA0;
			}
		}
		File.WriteAllBytes(outF, bufferFile);
	}

	private static void Decode(string inF, string outF)
	{
		byte[] bufferFile = File.ReadAllBytes(inF);
		for (int i = 0; i < bufferFile.Length; i++)
		{
			unchecked
			{
				bufferFile[i] -= 0xA0;
			}
		}
		File.WriteAllBytes(outF, bufferFile);
	}
}
