

public class GameBoardLogics {

	// fields:
	protected Tile [][] board;
	int SIZE = 4;
	private boolean leftPossible;
	private boolean rightPossible;
	private boolean upPossible;
	private boolean downPossible;
	private Integer score;
	
	
	// constructor: 
	
	public GameBoardLogics(Tile[][] board)
	{
		this.board= board;
		updatePossiblities();
		this.score = 0;
	}
	
	public GameBoardLogics(TileBank itsTileBank)
	{
		Tile[][] newBoard = new Tile[4][4];
		this.board = buildNewBoard(newBoard, itsTileBank);
		updatePossiblities();
		this.score = 0;
	}
	
	// methods:
	
	public Tile[][] buildNewBoard (Tile[][] board, TileBank itsTileBank)
	{
		this.board = board;
		// build empty board:
		this.board = new Tile[SIZE][SIZE];
		for (int i = 0 ;  i<= SIZE-1  ; i++ )
		{
			for (int j = 0 ;  j<= SIZE-1  ; j++ )
			{
				// Initialize the board
				this.board[i][j]=  TileBank.tile_Empty;
			}
		}		
		// randomize a place for the first tile:
		addNewRandomTile(this.board, itsTileBank);
		addNewRandomTile(this.board, itsTileBank);
		updatePossiblities();
		this.score = 0;
			return this.board;
		
	}
	
	//checks if the player reached 2048 tile
	public boolean IsWinning (Tile[][] board)
	{
		this.board = board;
		int num = 0 ; 
		for (int  i = 0 ; i<=SIZE-1 ; i++ )
		{
			for ( int j = 0 ; j <= SIZE-1 ; j++)
			{
				num =this.board[i][j].getNumOfTile();
				if (num == 2048 )
				{
					return true;
				}
			}
		}
		return false;
	}
	
	public boolean isBoardFull (Tile[][] board)
	 {
		this.board = board;
		 int num = 0;
		 Tile tile;
		 for (int  i = 0 ; i<=SIZE-1 ; i++ )
			{
				for ( int j = 0 ; j <= SIZE - 1 ; j++)
				{
					tile = this.board[i][j];
					num= tile.getNumOfTile();
					if (num == 0 )
					{
						return false;
					}
				}
			}
		 return true;
	 }
	
	//checks if there are possible moves
	public boolean isThereMoves (Tile[][] board)
	{
		updatePossiblities();
		return(leftPossible | rightPossible | upPossible | downPossible);
	}
	
	
	public void addNewRandomTile (Tile[][] board, TileBank itsTileBank)
	 {
		this.board=board;
		if (!isBoardFull(this.board))
		{
			// randomize a place for the random tile:
			double randomTile = Math.random();
			Tile random_Tile = TileBank.tile_Empty;
			if (randomTile <= 0.75)
				random_Tile = itsTileBank.tile2;
			else
				random_Tile = itsTileBank.tile4;
					
			int IndexRow = (int)(Math.random()*4 );
			int IndexCol = (int)(Math.random()*4 );
		
			boolean taken = false;
			if (this.board[IndexCol][IndexRow].getNumOfTile() != 0)
				// this place is taken
			{
				taken = true;
			}
			while (taken)
			{
				IndexRow = (int)(Math.random()*4 );
				IndexCol = (int)(Math.random()*4 );
				
				if (this.board[IndexCol][IndexRow].getNumOfTile() != 0)
					{
					taken = true;
					}
				 else
					{
					 taken= false;
					}
			}
			
			
			this.board[IndexCol][IndexRow] =  random_Tile;
		}			
	 }
	 
	//moves the board down
	public boolean BoardDown(Tile[][] board, TileBank itsTileBank)
	{
		if (!downPossible)
			return false;
		else
		{
			for (int i = 0; i < 4; i++)
				for (int j = 2; j >= 0; j--)
					if (board[i][j].numOfTile != 0)
					{
						Tile currentTile = board[i][j];
						board[i][j] = TileBank.tile_Empty;
						int m = j;
						while (m < 2 && board[i][m + 1].numOfTile == 0)
						{
							m++;
						}
						if (board[i][m + 1].numOfTile == 0)
						{
							board[i][m + 1] = currentTile;
						}
						else if (board[i][m + 1].numOfTile == currentTile.numOfTile)
						{
							board[i][m + 1] = board[i][m + 1].next;
							this.score = this.score + board[i][m + 1].numOfTile;
							board[i][m + 1].isAresult = true;
						}
						else board[i][m] = currentTile;
					}

			addNewRandomTile(board, itsTileBank);
			initialiseBoardToFalse(board);
			updatePossiblities();
			return true;
		}
	}
	
	//moves the board up
	public boolean BoardUp (Tile[][] board, TileBank itsTileBank)
	{
		if (!upPossible)
			return false;
		else
		{
			for (int i = 0; i <= 3; i++)
				for (int j = 1; j <= 3; j++)
					if (board[i][j].numOfTile != 0)
					{
						Tile currentTile = board[i][j];
						board[i][j] = TileBank.tile_Empty;
						int m = j;
						while (m > 1 && board[i][m - 1].numOfTile == 0)
						{
							m--;
						}
						if (board[i][m - 1].numOfTile == 0)
						{
							board[i][m - 1] = currentTile;
						}
						else if (board[i][m - 1].numOfTile == currentTile.numOfTile && !board[i][m - 1].isAresult)
						{
							board[i][m - 1] = board[i][m - 1].next;
							this.score = this.score + board[i][m - 1].numOfTile;
							board[i][m - 1].isAresult = true;
						}
						else board[i][m] = currentTile;
					}

			
			addNewRandomTile(board, itsTileBank);
			initialiseBoardToFalse(board);
			updatePossiblities();
			return true;
		}
	}
	
	//moves the board left
	public boolean BoardLeft  (Tile[][] board, TileBank itsTileBank)
	{
		if (!leftPossible)
			return false;
		else
		{
			for (int j = 1; j <= 3; j++)
				for (int i = 0; i <= 3; i++)
					if (board[j][i].numOfTile != 0)
					{
						Tile currentTile = board[j][i];
						board[j][i] = TileBank.tile_Empty;
						int m = j;
						while (m > 1 && board[m - 1][i].numOfTile == 0)
						{
							m--;
						}
						if (board[m - 1][i].numOfTile == 0)
						{
							board[m - 1][i] = currentTile;
						}
						else if (board[m - 1][i].numOfTile == currentTile.numOfTile && !board[m - 1][i].isAresult)
						{
							board[m - 1][i] = board[m - 1][i].next;
							this.score = this.score + board[m - 1][i].numOfTile;
							board[m - 1][i].isAresult = true;
						}
						else board[m][i] = currentTile;
					}

			
			addNewRandomTile(board, itsTileBank);
			initialiseBoardToFalse(board);
			updatePossiblities();
			return true;
		}
	}

	
	//moves the board right
	public boolean BoardRight  (Tile[][] board, TileBank itsTileBank)	
		{
			if (!rightPossible)
				return false;
			else
			{
				for (int j = 2; j >= 0; j--)
					for (int i = 0; i < 4; i++)
						if (board[j][i].numOfTile != 0)
						{
							Tile currentTile = board[j][i];
							board[j][i] = TileBank.tile_Empty;
							int m = j;
							while (m < 2 && board[m + 1][i].numOfTile == 0)
							{
								m++;
							}
							if (board[m + 1][i].numOfTile == 0)
							{
								board[m + 1][i] = currentTile;
							}
							else if (board[m + 1][i].numOfTile == currentTile.numOfTile)
							{
								board[m + 1][i] = board[m + 1][i].next;
								this.score = this.score + board[m + 1][i].numOfTile;
								board[m + 1][i].isAresult = true;
							}
							else board[m][i] = currentTile;
						}

				addNewRandomTile(board, itsTileBank);
				initialiseBoardToFalse(board);
				updatePossiblities();
				return true;
			}
	}

	
	public Tile[][] copyBoard(Tile[][] board)
	{
		Tile[][] newBoard = new Tile [SIZE][SIZE];
		for (int i=0 ; i<=SIZE-1 ; i++ )
		{
			for ( int j= 0 ; j<=SIZE-1 ; j++ )
			{
				newBoard[i][j] =this.board[i][j];	
			}
		}
		return newBoard;
	}
	
	//sets the is result field to false in all of the tiles
	 public void initialiseBoardToFalse  (Tile[][] board)
	 {
		 for (int i=0 ; i<=SIZE-1 ; i++ )
			{
				for ( int j= 0 ; j<=SIZE-1 ; j++ )
				{
					if (board[i][j].getNumOfTile()!=0)
					{
						board[i][j].isAresult = false;
					}
				}
			}
	 }
	 
	 
	 public String getScore()
	 {
		 return this.score.toString();
	 }
	 
	 private void updatePossiblities()
	 {
		 int tileCount = 0;
		//check down:
		 downPossible = false;
		 for (int i = 0; i < 4 & !downPossible; i++)
		 {
			 for (int j = 0; j < 3 & !downPossible; j++)
			 {
				 if (board[i][j].numOfTile != 0)
					 if (board[i][j + 1].numOfTile == 0 | board[i][j + 1].numOfTile == board[i][j].numOfTile)
						 downPossible = true;
					 else
						 tileCount++;
			 }
		 }
		 if (!downPossible & tileCount == 0)
			 downPossible = true;
		//check up:
		 tileCount = 0;
		 upPossible = false;
		 for (int i = 0; i < 4 & !upPossible; i++)
		 {
			 for (int j = 1; j < 4 & !upPossible; j++)
			 {
				 if (board[i][j].numOfTile != 0)
					 if (board[i][j - 1].numOfTile == 0 | board[i][j - 1].numOfTile == board[i][j].numOfTile)
						 upPossible = true;
					 else
						 tileCount++;
			 }
		 }
		 if (!upPossible & tileCount == 0)
			 upPossible = true;
		//check right:
		 tileCount = 0;
		 rightPossible = false;
		 for (int i = 0; i < 3 & !rightPossible; i++)
		 {
			 for (int j = 0; j < 4 & !rightPossible; j++)
			 {
				 if (board[i][j].numOfTile != 0)
					 if (board[i + 1][j].numOfTile == 0 | board[i + 1][j].numOfTile == board[i][j].numOfTile)
						 rightPossible = true;
					 else
						 tileCount++;
			 }
		 }
		 if (!rightPossible & tileCount == 0)
			 rightPossible = true;
		//check left:
		 tileCount = 0;
		 leftPossible = false;
		 for (int i = 1; i < 4 & !leftPossible; i++)
		 {
			 for (int j = 0; j < 4 & !leftPossible; j++)
			 {
				 if (board[i][j].numOfTile != 0)
					 if (board[i - 1][j].numOfTile == 0 | board[i - 1][j].numOfTile == board[i][j].numOfTile)
						 leftPossible = true;
					 else
						 tileCount++;
			 }
		 }
		 if (!leftPossible & tileCount == 0)
			 leftPossible = true;
	 }
}
