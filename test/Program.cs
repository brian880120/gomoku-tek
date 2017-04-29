using System;

namespace ConsoleApp1
{
	class Move
	{
		public int colIndex;
		public int rowIndex;
		public String color;

		public Move(int row, int col, String colorIn)
		{
			this.colIndex = col;
			this.rowIndex = row;
			color = colorIn;
		}
	}

    class Program
    {
        static void Main(string[] args)
        {
			Move newMove = new Move(4, 3, "white");
			Move w1 = new Move(2, 4, "white"); Move w3 = new Move(3, 6, "white"); Move w5 = new Move(1, 3, "white");
			Move w2 = new Move(5, 7, "white"); Move w4 = new Move(4, 6, "white"); Move w6 = new Move(6, 6, "white");

			Move b1 = new Move(2, 5, "black"); Move b3 = new Move(3, 4, "black"); Move b5 = new Move(4, 5, "black"); Move b7 = new Move(4, 2, "black"); //Move b9 = new Move(4, 2, "black");
			Move b2 = new Move(3, 3, "black"); Move b4 = new Move(4, 4, "black"); Move b6 = new Move(5, 2, "black"); Move b8 = new Move(6, 1, "black");

			Move[] occupiedPosition = {w1, b1, w2, b2, w3, b3, w4, b4, w5, b5, w6, b6, b7, b8 };


			//Console.WriteLine(CheckHorizontal(newMove, occupiedPosition));
			//Console.WriteLine(CheckVertical(newMove, occupiedPosition));
			//Console.WriteLine(CheckForwardSlash(newMove, occupiedPosition));
			//Console.WriteLine(CheckBackwardSlash(newMove, occupiedPosition));
           // Console.WriteLine("Hello World!");
			
        }

		public static Boolean IsSame(int row, int col, String color, Move[] occupiedPosition) {
			for (int i = 0; i < occupiedPosition.Length; i++)
			{
				if (row == occupiedPosition[i].rowIndex && col == occupiedPosition[i].colIndex && String.Equals(color, occupiedPosition[i].color))
					//Console.WriteLine("array length  "+ occupiedPosition.Length + "  " + row + "  " + col + "  " + color);
					return true;
			}
			return false;
		}

		public static Boolean CheckHorizontal(Move move, Move[] occupiedPosition){
			int counter = 1;
			int col = move.colIndex;	int row = move.rowIndex;	String color = move.color;

			for (int i = col + 1; i <= 18; i++)
			{
				if (IsSame(row, i, color, occupiedPosition))
					counter++;
				else
					break;
			}

			for (int i = col - 1; i >= 1; i--)
			{
				if (IsSame(row, i, color, occupiedPosition))
					counter++;
				else
					break;
			}

			if (counter == 5)
				return true;
			return false;
		}


		public static Boolean CheckVertical(Move move, Move[] occupiedPosition)
		{
			int counter = 1;
			int col = move.colIndex; int row = move.rowIndex; String color = move.color;

			for (int i = row + 1; i <= 15; i++)
			{
				if (IsSame(i, col, color, occupiedPosition))
					counter++;
				else
					break;
			}

			for (int i = row - 1; i >= 1; i--)
			{
				if (IsSame(i, col, color, occupiedPosition))
					counter++;
				else
					break;
			}

			if (counter == 5)
				return true;
			return false;
		}



		public static Boolean CheckForwardSlash(Move move, Move[] occupiedPosition)
		{
			int counter = 1;
			int col = move.colIndex; int row = move.rowIndex; String color = move.color;
			int j = row + 1;	int k = row - 1;

			for (int i = col + 1; i <= 18; i++)
			{
				if (j <= 15)
				{
					if (IsSame(j, i, color, occupiedPosition))
						counter++;
					else
						break;
				}
				else
					break;
				j++;
			}

			for (int i = col - 1; i >= 1; i--)
			{
				if (k >= 1)
				{
					if (IsSame(k, i, color, occupiedPosition))
						counter++;
					else
						break;
				}
				else
					break;
				k--;
			}

			if (counter == 5)
				return true;

			return false;
		}


		public static Boolean CheckBackwardSlash(Move move, Move[] occupiedPosition)
		{
			int counter = 1;
			int col = move.colIndex; int row = move.rowIndex; String color = move.color;
			int j = row - 1; int k = row + 1;

			for (int i = col + 1; i <= 18; i++)
			{
				if (j >= 1)
				{
					if (IsSame(j, i, color, occupiedPosition))
						counter++;
					else
						break;
				}
				else
					break;
				j--;
			}

			for (int i = col - 1; i >= 1; i--)
			{
				if (k <= 15)
				{
					if (IsSame(k, i, color, occupiedPosition))
						counter++;
					else
						break;
				}
				else
					break;
				k++;
			}

			if (counter == 5)
				return true;

			return false;
		}



	}
}