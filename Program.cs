using System;

class Program
{
    static void Main()
    {
        Console.WriteLine("It''s time to play!");
        Console.WriteLine("");
        Play_Game();
    }

    static int Calculate_Empty_Blocks(int[,] board)
    {
        int num_empty_blocks = 0;

        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                if (board[i, j] == 0)
                {
                    num_empty_blocks += 1;
                }
            }
        }
        return num_empty_blocks;

    }

    static int Select_Random_Num(int min, int max)
    {
        Random rnd = new Random();
        return rnd.Next(min, max + 1);
    }

    static void Add_Random_New_Block(int[,] board)
    {
        int num_empty_blocks = Calculate_Empty_Blocks(board);

        int new_block_index = Select_Random_Num(1, num_empty_blocks);

        int[] potential_block_values = { 2, 4 };

        int random_block_value;

        if (Select_Random_Num(1, 100) > 10)
        {
            random_block_value = 2;
        }
        else
        {
            random_block_value = 4;
        }

        int nth_empty_block = 0;

        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                if (board[i, j] == 0)
                {
                    nth_empty_block += 1;
                }

                if (nth_empty_block == new_block_index)
                {
                    board[i, j] = random_block_value;
                    return;
                }
            }
        }

    }

    static bool Board_Changed(int[,] prev_board, int[,] board)
    {
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                if (prev_board[i, j] != board[i, j])
                {
                    return true;
                }
            }
        }
        return false;
    }

    static void Process_Move(int[,] board, char player_move, ref int score, ref int num_moves)
    {
        int[,] prev_board = (int[,])board.Clone();
        if (player_move == 'w')
        {

            int[] run = { 0, 0, 0, 0 };

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    run[j] = board[j, i];
                }
                Process_Run(run, ref score);

                for (int j = 0; j < 4; j++)
                {
                    board[j, i] = run[j];
                }
            }
        }
        else if (player_move == 's')
        {
            int[] run = { 0, 0, 0, 0 };

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    run[3 - j] = board[j, i];
                }
                Process_Run(run, ref score);
                for (int j = 0; j < 4; j++)
                {
                    board[j, i] = run[3 - j];
                }
            }
        }
        else if (player_move == 'a')
        {
            int[] run = { 0, 0, 0, 0 };

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    run[j] = board[i, j];
                }
                Process_Run(run, ref score);
                for (int j = 0; j < 4; j++)
                {
                    board[i, j] = run[j];
                }
            }
        }
        else if (player_move == 'd')
        {
            int[] run = { 0, 0, 0, 0 };

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    run[3 - j] = board[i, j];
                }
                Process_Run(run, ref score);
                for (int j = 0; j < 4; j++)
                {
                    board[i, j] = run[3 - j];
                }
            }
        }



        // Now we need to add a random 2 to an empty space in the board
        // do this by calculating how many 0s there are
        // now pick a random number in this range
        // replace the nth number with a 2


        if (Board_Changed(prev_board, board))
        {
            Add_Random_New_Block(board);
            num_moves += 1;
        }

        return;
    }

    static void Process_Run(int[] arr, ref int score)
    {

        // combine numbers that are equal
        for (int i = 0; i < 4; i++)
        {
            for (int j = i + 1; j < 4; j++)
            {
                if (arr[i] == arr[j])
                {
                    arr[i] = arr[i] + arr[j];
                    score += arr[i];
                    arr[j] = 0;
                    break;
                }
            }
        }

        int front = 0;
        int back = 0;

        while (front < 4 && back < 4)
        {
            if (arr[front] != 0)
            {
                front += 1;
                back = front + 1;
            }
            else if (arr[back] == 0)
            {
                back += 1;
            }
            else if (arr[back] != 0)
            {
                arr[front] = arr[back];
                arr[back] = 0;
            }
        }

    }

    static void Play_Game()
    {
        int score = 0;
        // int[,] board = { { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 } };
        int[,] board = { { 16, 0, 0, 0 }, { 8, 0, 0, 0 }, { 8, 0, 0, 0 }, { 0, 0, 0, 0 } };

        int num_moves = 0;

        // Add_Random_New_Block(board);
        // Add_Random_New_Block(board);
        // int[,] board = { { 2, 2, 0, 0 }, { 4, 2, 0, 0 }, { 0, 2, 0, 0 }, { 4, 2, 0, 0 } };
        Console.WriteLine("Make a move using W A S D");
        while (!Game_Over(board))
        {
            Display_Board(board);
            Console.WriteLine();


            char player_move = Console.ReadKey(true).KeyChar;

            // string player_move = Console.ReadLine();
            if (player_move != 'w' && player_move != 'a' && player_move != 's' && player_move != 'd')
            {
                continue;
            }
            Process_Move(board, player_move, ref score, ref num_moves);
        }


        Display_Board(board);

        Console.BackgroundColor = ConsoleColor.Black;
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine($"You Scored {score} points in {num_moves} moves");
    }

    static bool Game_Over(int[,] board)
    {
        // The game ends when there are no legal moves left
        // Go through each piece on the board
        // If any piece has an adjacent piece that is the same or is 0 then there are still legal moves left

        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                if (board[i, j] == 0)
                {
                    return false;
                }
                // check above

                else if (i - 1 > 0 && board[i, j] == board[i - 1, j])
                {
                    return false;
                }

                // check below
                else if (i + 1 < 4 && board[i, j] == board[i + 1, j])
                {
                    return false;
                }

                // check left

                else if (j - 1 > 0 && board[i, j] == board[i, j - 1])
                {
                    return false;
                }

                // check right
                else if (j + 1 < 4 && board[i, j] == board[i, j + 1])
                {
                    return false;
                }
            }
        }
        return true;
    }

    static void Set_Console_Background_Colour(int block_value = -1)
    {

        switch (block_value)
        {
            case -2:
                Console.BackgroundColor = ConsoleColor.Black;
                break;
            case -1:
                Console.BackgroundColor = ConsoleColor.White;
                break;
            case 0:
                Console.BackgroundColor = ConsoleColor.DarkGray;
                break;
            case 2:
                Console.BackgroundColor = ConsoleColor.DarkCyan;
                break;
            case 4:
                Console.BackgroundColor = ConsoleColor.Cyan;
                break;
            case 8:
                Console.BackgroundColor = ConsoleColor.Blue;
                break;
            case 16:
                Console.BackgroundColor = ConsoleColor.DarkBlue;
                break;
            case 32:
                Console.BackgroundColor = ConsoleColor.DarkYellow;
                break;
            case 64:
                Console.BackgroundColor = ConsoleColor.Yellow;
                break;
            case 128:
                Console.BackgroundColor = ConsoleColor.DarkMagenta;
                break;
            case 256:
                Console.BackgroundColor = ConsoleColor.Magenta;
                break;
            case 512:
                Console.BackgroundColor = ConsoleColor.DarkRed;
                break;
            case 1024:
                Console.BackgroundColor = ConsoleColor.Red;
                break;
            case 2048:
                Console.BackgroundColor = ConsoleColor.Green;
                break;
        }
    }

    static void Display_Board(int[,] board)
    {
        // borders between boxes are made using #'s

        for (int i = 0; i < 4; i++)
        {
            Set_Console_Background_Colour();
            if (i == 0)
            {
                // First line of display
                Console.Write(String.Concat(Enumerable.Repeat(" ", 5 + 5 * 4)));
                Set_Console_Background_Colour(-2);
                Console.Write("\n");
            }

            for (int k = 0; k < 3; k++)
            {
                Set_Console_Background_Colour();
                for (int j = 0; j < 4; j++)
                {

                    int current_block_val = board[i, j];

                    if (j == 0)
                    {
                        Console.Write(" ");
                    }

                    if (k == 1)
                    {
                        Set_Console_Background_Colour(current_block_val);
                        Console.Write(current_block_val.ToString().PadRight(5));
                        Set_Console_Background_Colour();
                        Console.Write(" ");
                        Set_Console_Background_Colour(-2);
                    }

                    else
                    {
                        Set_Console_Background_Colour(current_block_val);
                        Console.Write("     ");
                        Set_Console_Background_Colour();
                        Console.Write(" ");
                        Set_Console_Background_Colour(-2);
                    }

                    if (j == 3)
                    {
                        Set_Console_Background_Colour(-2);
                        Console.Write("\n");
                    }
                }
                Set_Console_Background_Colour(-2);
            }
            Set_Console_Background_Colour();
            Console.Write(String.Concat(Enumerable.Repeat(" ", 5 + 5 * 4)));
            Set_Console_Background_Colour(-2);
            Console.Write("\n");
        }
        Set_Console_Background_Colour(-2);

    }
}
