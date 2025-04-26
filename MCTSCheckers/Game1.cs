using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using NeuralNetworks;
using MonoGame.Extended;
using MonteNode = NeuralNetworks.MonteCarloTree<MCTSCheckers.CheckersGameState>.Node;
using Piece = MCTSCheckers.CheckersGameState.Piece;

using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Linq;
using System.Diagnostics;

namespace MCTSCheckers;

public class Game1 : Game
{
    private GraphicsDeviceManager graphics;
    private SpriteBatch spriteBatch;

    Rectangle[,] board;
    Piece[,] originalBoard;
    MonteCarloTree<CheckersGameState> tree;
    bool redTurn = false;
    const int squareSize = 100;
    MouseState previousMs;
    List<Move> currentPossibleMoves;
    Random random;

    public Game1()
    {
        graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

    }

    protected override void Initialize()
    {
        tree = new MonteCarloTree<CheckersGameState>();
        board = new Rectangle[8, 8];
        originalBoard = new Piece[8, 8];
        Size ClientSize = new Size(squareSize * board.GetLength(0), squareSize * board.GetLength(0));
        //previousMs = Mouse.GetState();
        random = new Random();

        graphics.PreferredBackBufferWidth = ClientSize.Width;
        graphics.PreferredBackBufferHeight = ClientSize.Height;
        graphics.ApplyChanges();

        for (int i = 0; i < board.GetLength(0); i++)
        {
            for (int j = 0; j < board.GetLength(1); j++)
            {
                board[i, j] = new Rectangle(squareSize * i, squareSize * j, squareSize, squareSize);
                if (i % 2 != j % 2)
                {
                    if (j < 3)
                    {
                        originalBoard[i, j] = Piece.RedPiece;
                    }
                    else if (j > 4)
                    {
                        originalBoard[i, j] = Piece.BlackPiece;
                    }
                    else
                    {
                        originalBoard[i, j] = Piece.None;
                    }
                }
                else
                {
                    originalBoard[i, j] = Piece.None;
                }
            }
        }

        var originalState = new CheckersGameState(originalBoard, redTurn, 0);
        tree.root = new MonteNode(originalState, redTurn);
        currentPossibleMoves = [];


        var test = tree.root.State.GetChildren();


        base.Initialize();
    }

    protected override void LoadContent()
    {
        spriteBatch = new SpriteBatch(GraphicsDevice);

        // TODO: use this.Content to load your game content here
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        MouseState ms = Mouse.GetState();

        if ((ms.LeftButton != ButtonState.Pressed || previousMs.LeftButton != ButtonState.Released) || !GraphicsDevice.Viewport.Bounds.Contains(ms.Position)) return;

        int x = ms.X / 100;
        int y = ms.Y / 100;


        if (tree.root.State.IsTerminal)
        {
            ;
        }

        if (!redTurn)
        {
            if (tree.root.State.board[x, y] != Piece.None && !originalBoard[x, y].HasFlag(Piece.Red))
            {
                currentPossibleMoves = tree.root.State.board[x, y].GetPossibleMoves(tree.root.State.board, x, y).ToList();
            }

            else if (currentPossibleMoves.Count != 0)
            {
                foreach (var pair in currentPossibleMoves)
                {
                    if (board[pair.End.X, pair.End.Y].Contains(ms.X, ms.Y))
                    {
                        originalBoard = tree.root.State.board;
                        if (pair.End.Y == 0)
                        {
                            originalBoard[pair.End.X, pair.End.Y] = Piece.BlackKing; // eventual apply move function
                        }
                        else
                        {
                            originalBoard[pair.End.X, pair.End.Y] = originalBoard[pair.Start.X, pair.Start.Y]; // eventual apply move function
                        }
                        originalBoard[pair.Start.X, pair.Start.Y] = Piece.None;
                        if ((pair.End.X - pair.Start.X) % 2 == 0)
                        {
                            originalBoard[pair.Start.X + ((pair.End.X - pair.Start.X) / 2), pair.Start.Y + ((pair.End.Y - pair.Start.Y) / 2)] = Piece.None;
                            tree.root.State.Move = 0;
                        }

                        CheckersGameState newNode = new CheckersGameState(originalBoard, !redTurn, tree.root.State.Move);
                        tree.root.GenerateChildren();
                        foreach (var child in tree.root.Children)
                        {
                            if (child.State.board.SequenceEquals(originalBoard))
                            {
                                tree.root = child;
                                currentPossibleMoves.Clear();
                                redTurn = !redTurn;
                                //previousMs = ms;
                                return;
                            }
                        }
                    }
                }
            }

            if (tree.root.State.IsTerminal)
            {
                ;
            }

        }
        else
        {
            Debug.WriteLine("hi");
            var testState = tree.MCTS(50, tree.root.State, random);
            var testNode = new MonteNode(testState, testState.redTurn);

            for(int i = 0; i < board.GetLength(0); i++)
            {
                if (testNode.State.board[i, board.GetLength(1) - 1] == Piece.RedPiece)
                {
                    testNode.State.board[i, board.GetLength(1) - 1] = Piece.RedKing;
                }
            }

            foreach (var child in tree.root.State.children)
            {
                if (child == testNode.State)
                {
                    tree.root = testNode;
                    break;
                }
            }
            redTurn = !redTurn;
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.LightGray);

        spriteBatch.Begin();

        for (int i = 0; i < board.GetLength(0); i++)
        {
            for (int j = 0; j < board.GetLength(1); j++)
            {
                if (i % 2 != j % 2)
                {
                    spriteBatch.FillRectangle(board[i, j], Color.DarkGray);

                    if (tree.root.State.board[i, j].HasFlag(Piece.RedPiece))
                    {
                        spriteBatch.DrawCircle(new(new Vector2(board[i, j].X + squareSize / 2, board[i, j].Y + squareSize / 2), 25), 30, Color.Red, 25);
                    }
                    else if (tree.root.State.board[i, j].HasFlag(Piece.BlackPiece))
                    {
                        spriteBatch.DrawCircle(new(new Vector2(board[i, j].X + squareSize / 2, board[i, j].Y + squareSize / 2), 25), 30, Color.Black, 25);
                    }
                    if (tree.root.State.board[i, j].HasFlag(Piece.King))
                    {
                        spriteBatch.DrawCircle(new(new Vector2(board[i, j].X + squareSize / 2, board[i, j].Y + squareSize / 2), 25), 30, Color.Gold, 2);
                    }
                }
            }
        }
        if (currentPossibleMoves.Count != 0)
        {
            foreach (var pair in currentPossibleMoves)
            {
                spriteBatch.DrawCircle(new(new Vector2(board[pair.End.X, pair.End.Y].X + squareSize / 2, board[pair.End.X, pair.End.Y].Y + squareSize / 2), 25), 30, Color.Yellow, 25);
            }
        }




        spriteBatch.End();

        base.Draw(gameTime);
    }

    public List<int[]> PossibleMoves(int x, int y)
    {
        var currentBoard = tree.root.State.board;
        List<int[]> moves = new List<int[]>();

        var newMoves = currentBoard[x, y].GetPossibleMoves(currentBoard, x, y);



        return moves;
    }
}
