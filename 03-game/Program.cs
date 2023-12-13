var game = new TicTacToeGame();
var ai = new AiAssistant();
while (game.Winner == "")
{
    game.PrintBoardWithCoordinates();
    Console.WriteLine("What is your next move? (row, column)");
    var input = Console.ReadLine();
    var success = game.UpdateBoard(input, "X");

    if (success)
    {
        game = ai.MakeNextMove(game);
        Console.WriteLine("AI: " + game.Comment);
    }
    else
    {
        Console.WriteLine("Invalid move. Try again.");
    }
}

Console.WriteLine("The winner is: " + game.Winner);
