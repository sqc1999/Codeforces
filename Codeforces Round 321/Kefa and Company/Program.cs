﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Kefa_and_Company
{
	class TextReaderHelper
	{
		protected TextReader baseReader;
		protected LinkedList<string> buffer;

		public TextReaderHelper(TextReader baseReader)
		{
			this.baseReader = baseReader;
			buffer = new LinkedList<string>();
		}

		protected void readIfEmpty()
		{
			if (buffer.Count > 0) return;
			string line;
			do
			{
				line = baseReader.ReadLine();
				if (line == null) return;
			}
			while (line.Trim() == string.Empty);
			foreach (var item in line.Split(' ', '\n', '\t'))
				if (item != string.Empty)
					buffer.AddLast(item);
		}

		private object readChar()
		{
			var node = buffer.First;
			var ret = node.Value[0];
			if (node.Value.Length > 1) node.Value = node.Value.Substring(1);
			else buffer.RemoveFirst();
			return ret;
		}

		public T NextElement<T>()
		{
			readIfEmpty();
			if (typeof(T) == typeof(char)) return (T)readChar();
			var ret = (T)Convert.ChangeType(buffer.First.Value, typeof(T));
			buffer.RemoveFirst();
			return ret;
		}

		public int NextInt() { return NextElement<int>(); }

		public string NextString() { return NextElement<string>(); }
	}

	class Program
	{
		[Conditional("DEBUG")]
		private static void Pause()
		{
			Console.ReadKey();
		}

		static void Main(string[] args)
		{
			var reader = new TextReaderHelper(new StreamReader(Console.OpenStandardInput(), Encoding.ASCII, false, 1048576));
			var writer = new StreamWriter(Console.OpenStandardOutput(), Encoding.ASCII, 1048576);
			int n = reader.NextInt(), d = reader.NextInt();
			var a = new Friend[n];
			for (int i = 0; i < n; i++)
			{
				a[i].Money = reader.NextInt();
				a[i].FriendshipFactor = reader.NextInt();
			}
			Array.Sort(a);
			int l = 0, r = 1;
			long sum = a[0].FriendshipFactor;
			long ans = sum;
			while (r < n)
			{
				sum += a[r++].FriendshipFactor;
				while (a[r - 1].Money - a[l].Money >= d)
					sum -= a[l++].FriendshipFactor;
				ans = Math.Max(ans, sum);
			}
			writer.WriteLine(ans);
			writer.Flush();
			Pause();
		}
	}

	struct Friend : IComparable<Friend>
	{
		public int Money, FriendshipFactor;

		public int CompareTo(Friend other)
		{ return Money - other.Money; }

		public override string ToString()
		{ return string.Format("Money = {0}, FriendshipFactor = {1}", Money, FriendshipFactor); }
	}
}
