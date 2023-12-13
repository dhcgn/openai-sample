using Newtonsoft.Json;

public class TicTacToeGame
{
    [JsonProperty("board")]
    public string[][] Board { get; set; }

    [JsonProperty("winner")]
    public string Winner { get; set; }

    [JsonProperty("comment")]
    public string Comment { get; set; }

    public TicTacToeGame()
    {
        Board = [
            ["", "", ""],
            ["", "", ""],
            ["", "", ""]
        ];

        Winner = "";
        Comment = "";
    }

    public void PrintBoardWithCoordinates()
    {
        Console.WriteLine("  012");
        for (int i = 0; i < Board.Length; i++)
        {
            // Print the row number
            Console.Write(i + " ");
            for (int j = 0; j < Board[i].Length; j++)
            {
                // Print the cell value
                var field = Board[i][j];
                if (field == "")
                {
                    field = "-";
                }

                Console.Write(field);
            }
            Console.WriteLine();
        }
    }

    public bool UpdateBoard(string input, string symbol)
    {
        var coordinates = input.Split(",");
        if (coordinates.Length != 2)
        {
            return false;
        }

        if (!int.TryParse(coordinates[0], out int row) || !int.TryParse(coordinates[1], out int column))
        {
            return false;
        }

        return UpdateBoard(row, column, symbol);
    }
    public bool UpdateBoard(int row, int column, string symbol)
    {
        // Check if the coordinates are within the bounds of the board
        if (row >= 0 && row < 3 && column >= 0 && column < 3)
        {
            // Check if the cell is already occupied
            if (Board[row][column] == "")
            {
                Board[row][column] = symbol;
                return true; // Successfully updated the board
            }
            else
            {
                Console.WriteLine("That space is already occupied!");
                return false; // Cell is already occupied
            }
        }
        else
        {
            Console.WriteLine("Invalid coordinates. Please choose between [0, 2] for both row and column.");
            return false; // Invalid coordinates
        }
    }
}

