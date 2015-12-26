import javax.swing.ImageIcon;


public class Board {

	//attributes:
	final int SIZE = 9; 
	public Candy[][] board;
	
	
	//methods:
	

	//constructor:
	public Board()
	{
		Candy[][] board = new Candy[SIZE][SIZE];
		this.board = board;
		
		// Initialize an empty board
        for ( int i= 0 ; i < SIZE ; i++)
		{
			for (int j = 0 ; j < SIZE ; j++ )
			{
				board[j][i]= new RegularCandy(0,Candy_Bank.empty);
				board[j][i].setColumn(j);
				board[j][i].setRow(i);
				board[j][i].setBoard(board);
			}
		}
        
        this.fillRandom(this.board);
                
        boolean boardChanged = playAlone(this.board);
        
        while (boardChanged | EmptyCells(this.board))
        {

        	this.fillRandom(this.board);
        	
        	boardChanged = playAlone(board);
        	
        	killSpecial();
        	
        }

		
	}
	
	public boolean EmptyCells(Candy[][] board2) 
	// the function check if there are empty spaces in the board
	{
		this.board=board2;
		for ( int col = SIZE-1 ; col >= 0 ; col--)
		{
			for (int row = SIZE-1 ; row >= 0 ; row-- )
			{
				if (board[col][row].getColor()==0)
					return true;
			}
		}
		return false;
	}

	public boolean isBoardHasChanged (Candy[][] board1,Candy[][] board2)
	// the function check if the board are not the same , "true" for the board are not the same
	{
		for ( int col = SIZE-1 ; col >= 0 ; col--)
		{
			for (int row = SIZE-1 ; row >= 0 ; row-- )
			{
				if (board1[col][row].getColor()!= board2[col][row].getColor())
				{
					return true;
				}
			}
		}
		return false;
	}

	public boolean playAlone (Candy[][] board2)
	// the board play by itself , crushes all possible candies
	{	
		boolean changed = false;
		for (int i = 0; i <= SIZE - 1; i++)
			for (int j = 0; j <= SIZE - 1; j++)
			{
				if (board2[i][j].crushNearcandy())
					changed = true;
			}
		return changed;
	}
	
	public int updateScoreAfterMove(Candy[][] board2)
	//the function check for empty spaces:
	{
		int score = 0;
		if (EmptyCells(this.board))
		{
			for ( int col = SIZE-1 ; col >= 0 ; col--)
			{
				for (int row = SIZE-1 ; row >= 0 ; row-- )
				{
					if (board[col][row].getColor()==0)
					{
						score = score + 10;
					}
				}
			}
		}
		return score;
	}
	
	private void killSpecial ()
	// the function delete the special candies in the 
	// Beginning 
	{
		for ( int col = 0; col <SIZE ; col++)
		{
			for (int row = 0 ; row <SIZE ; row++ )
			{
				if (board[col][row].getColor()>9)
				{
					this.board[col][row] = new RegularCandy(0);
					this.board[col][row].setCandyIcon(Candy_Bank.empty);
					this.board[col][row].setColumn(col);
					this.board[col][row].setRow(row);
					this.board[col][row].setBoard(board);
				}

			}
		}
	}
	
	public void fillRandom(Candy[][] board2)
	/* puts new random candies in the 
 	free spaces in the board */ 
	{
		this.board=board2;

		int random ;
		for ( int col = SIZE-1 ; col >= 0 ; col--)
		{
			for (int row = SIZE-1 ; row >= 0 ; row-- )
			{
				if (this.board[col][row].getColor()==0)
					// the tile is empty
				{
					ImageIcon icon;
					random = (int)((Math.random()*6)+1);
					this.board[col][row]= new RegularCandy(random);
					switch (random)
					{
						case 1: 
						{
							icon=Candy_Bank.candy1;
							break;
						}
						case 2: 
						{
							icon=Candy_Bank.candy2;
							break;
						}
						case 3: 
						{
							icon=Candy_Bank.candy3;
							break;
						}
						case 4: 
						{
							icon=Candy_Bank.candy4;
							break;
						}
						case 5: 
						{
							icon=Candy_Bank.candy5;
							break;
						}
						case 6: 
						{
							icon=Candy_Bank.candy6;
							break;
						}
						default:
						{
							icon=Candy_Bank.candy6;
							break;
						}
					}
					this.board[col][row].setCandyIcon(icon);
					this.board[col][row].setColumn(col);
					this.board[col][row].setRow(row);
					this.board[col][row].setBoard(board);
				}
					
			}
		}
	}

	public boolean boardDown(Candy[][] board)
	// makes all the candy fall down to fill empty spaces
	{
		boolean changed = false;
		for (int col=SIZE-1 ; col>=0 ; col--)
		{
			for (int row=SIZE-2 ; row>=0 ; row--)
			{
				if (board[col][row].getColor()!=0 && board[col][row+1].getColor()==0)
				{
					board[col][row + 1] = board[col][row];
					board[col][row] = new RegularCandy(0);
					changed = true;
				}
			}
		}
		for (int col = 0; col <= SIZE - 1; col++)
		{
			if (this.board[col][0].getColor()==0)
				// the tile is empty
			{
				ImageIcon icon;
				int random = (int)((Math.random()*6)+1);
				this.board[col][0]= new RegularCandy(random);
				switch (random)
				{
					case 1: 
					{
						icon=Candy_Bank.candy1;
						break;
					}
					case 2: 
					{
						icon=Candy_Bank.candy2;
						break;
					}
					case 3: 
					{
						icon=Candy_Bank.candy3;
						break;
					}
					case 4: 
					{
						icon=Candy_Bank.candy4;
						break;
					}
					case 5: 
					{
						icon=Candy_Bank.candy5;
						break;
					}
					case 6: 
					{
						icon=Candy_Bank.candy6;
						break;
					}
					default:
					{
						icon=Candy_Bank.candy6;
						break;
					}
				}
				this.board[col][0].setCandyIcon(icon);
				this.board[col][0].setColumn(col);
				this.board[col][0].setRow(0);
				this.board[col][0].setBoard(board);
			}
		}
		
		return changed;
	}
	
	public boolean isThereAreExplosionsInColumns (Candy[][] board2)
	// check is there are any crushes to make in columns
	{
		this.board=board2;
		for ( int column= 0 ; column <= SIZE-1 ; column++)
		{
			for (int row = 0 ; row <= SIZE-1 ; row++ )
			{
				// checking columns:
				if ( (row+2<=SIZE-1) && (board[column][row].getColor()
						== board[column][row+1].getColor() )
						&& 
						(board[column][row+1].getColor()
						== board[column][row+2].getColor()) )
					return true;
			}
		}
		return false;
	}
	
	public boolean isThereAreExplosionsInRows (Candy[][] board2)
	// check is there are any crushes to make in rows
	{
		this.board=board2;
		for (int row = 0 ; row <= SIZE-1 ; row++ )
		{
			for  ( int column= 0 ; column <= SIZE-1 ; column++)
			{
				// checking rows:
				if ( (column + 2 <= SIZE-1) && (board[column][row].getColor()
						== board[column+1][row].getColor() )
						&& 
						(board[column+1][row].getColor()
						== board[column+2][row].getColor()) )
					return true;
			}
		}
		return false;
	}
	
	public Candy[][] getBoard ()
	{
		return this.board;
	}
}

