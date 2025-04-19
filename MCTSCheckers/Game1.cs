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
    List<int[]> currentPossibleMoves;
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

        var originalState = new CheckersGameState(originalBoard, redTurn);
        tree.root = new MonteNode(originalState, redTurn);
        currentPossibleMoves = new List<int[]>();


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

        if (!redTurn)
        {
            if (tree.root.State.board[x, y] != Piece.None && !originalBoard[x, y].HasFlag(Piece.Red))
            {
                currentPossibleMoves = tree.root.State.board[x, y].GetPossibleMoves(tree.root.State.board, x, y).ToList();
                //foreach (var move in moves)
                //{
                //    currentPossibleMoves.Add([x + move[0], y + move[1], x, y]);
                //}

            }

            else if (currentPossibleMoves.Count != 0)
            {
                foreach (var pair in currentPossibleMoves)
                {
                    if (board[pair[0], pair[1]].Contains(ms.X, ms.Y))
                    {
                        originalBoard = tree.root.State.board;
                        if (pair[1] == 0)
                        {
                            originalBoard[pair[0], pair[1]] = Piece.BlackKing; // eventual apply move function
                        }
                        else
                        {
                            originalBoard[pair[0], pair[1]] = originalBoard[pair[2], pair[3]]; // eventual apply move function
                        }
                        originalBoard[pair[2], pair[3]] = Piece.None;
                        if ((pair[0] - pair[2]) % 2 == 0)
                        {
                            originalBoard[pair[2] + ((pair[0] - pair[2]) / 2), pair[3] + ((pair[1] - pair[3]) / 2)] = Piece.None;
                        }

                        CheckersGameState newNode = new CheckersGameState(originalBoard, !redTurn);
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

        }
        else
        {
            var testState = tree.MCTS(100, tree.root.State, random);
            var testNode = new MonteNode(testState, testState.redTurn);

            foreach (var child in tree.root.State.GetChildren())
            {
                if (child == testNode.State)
                {
                    tree.root = testNode;
                    break;
                }
            }
            redTurn = !redTurn;
        }

        //previousMs = ms;

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
                }
            }
        }
        if (currentPossibleMoves.Count != 0)
        {
            foreach (var pair in currentPossibleMoves)
            {
                spriteBatch.DrawCircle(new(new Vector2(board[pair[0], pair[1]].X + squareSize / 2, board[pair[0], pair[1]].Y + squareSize / 2), 25), 30, Color.Yellow, 25);
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
