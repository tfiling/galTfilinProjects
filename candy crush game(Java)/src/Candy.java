import javax.swing.ImageIcon;


public abstract class Candy implements visitor,visited{
	
	// attributes:
	protected int color ;
	private ImageIcon candyIcon;
	final int SIZE = 9;
	protected Candy[][] board;
	protected int row;
	protected int column;
	

	// methods:
	
	//constructor:
	public Candy(int color)
	{
		this.setColor(color);
	}
	
	public Candy(int color, ImageIcon candyIcon)
	{
		this.setColor(color);
		this.setCandyIcon(candyIcon);
	}

	
	public Candy()
	// candy with color 0 represent an empty tile
	{
		this.setColor(0);
	}

	public abstract void crush();
	
	public boolean crushNearcandy()
	//the function crush in row or columns or 
	// combinations around the candy to crush
	{
		
		if (this.color != 0)
		{
			// find in the row
			int countLeft = -1;
			int countRight = -1;
			
			int row= this.getRow();
			int col= this.getColumn();
			
			int thisColor = this.color;
			if (this.color >= 10)
				thisColor = this.color/10;
			
			while ( col<SIZE && (board[col][row].getColor()%10 == thisColor && board[col][row].getColor()/10  == 0 ||//regular candy
								(board[col][row].getColor()/10  == thisColor && board[col][row].getColor() % 10 != 0))) //special candy (except bomb)
				// also wrapped and striped candies can crush
			{
				countRight ++;
				col++;
			}
			
			col= this.getColumn();
			
			while ( col>=0 && (board[col][row].getColor()%10 == thisColor && board[col][row].getColor()/10  == 0 ||//regular candy
					(board[col][row].getColor()/10  == thisColor && board[col][row].getColor() % 10 != 0))) //special candy (except bomb)
				// also wrapped and striped candies can crush
			{
				countLeft++;
				col--;
			}
			
			// find in the col
			int countUp = -1;
			int countDown = -1;
			
			row= this.getRow();
			col= this.getColumn();
			
			while ( row<SIZE && (board[col][row].getColor()%10 == thisColor && board[col][row].getColor()/10  == 0 ||//regular candy
					(board[col][row].getColor()/10  == thisColor && board[col][row].getColor() % 10 != 0))) //special candy (except bomb)
			{
				countDown ++;
				row++;
			}
			
			row= this.getRow();
			
			while ( row>=0 && (board[col][row].getColor()%10 == thisColor && board[col][row].getColor()/10  == 0 ||//regular candy
					(board[col][row].getColor()/10  == thisColor && board[col][row].getColor() % 10 != 0))) //special candy (except bomb)
			{
				countUp++;
				row--;
			}
	
			// check for "L" shape : 
			if 		(countRight + countDown == 4 
					|| countUp + countLeft == 4 
					|| countRight + countUp == 4 
					|| countLeft + countDown == 4 )
			{
				//there is a "L" shape
				crushColorInRow(countLeft , countRight);
				crushColorIncol(countUp , countDown);
				// color of wrapped is 23,43 .. = 3 in the end 
				BagCandy newBag = new BagCandy(thisColor*10 +3);
				newBag.setColumn(this.getColumn());
				newBag.setRow(this.getRow());
				newBag.setBoard(board);
				board[this.getColumn()][this.getRow()] = newBag;
				return true;
			}
			
			//else: 
			//crush in a row:
			int sum = countLeft+countRight;
			switch (sum)
			{
				case 2: 
					// 3 of a kind
					{
						crushColorInRow(countLeft , countRight);
						return true; // exit the function
					}
				case 3:
					// 4 of a kind
					// horizontal striped bags
					{
						crushColorInRow(countLeft , countRight);
						HorizontalStripedCandy newhorizontalStriped = new HorizontalStripedCandy(thisColor*10 +2);
						// color of horizontal is 22,42 .. =2 in the end 
						newhorizontalStriped.setColumn(this.getColumn());
						newhorizontalStriped.setRow(this.getRow());
						newhorizontalStriped.setBoard(board);
						newhorizontalStriped.setCandyIcon(Candy_Bank.getIcon(thisColor));
						board[this.getColumn()][this.getRow()] = newhorizontalStriped;
						return true; // exit the function
					}
				case 4: 
					// 5 of a kind
					// color bomb
					{
						crushColorInRow(countLeft , countRight);
						BombCandy colorBomb = new BombCandy(10);
						colorBomb.setRow(this.getRow());
						colorBomb.setColumn(this.getColumn());
						colorBomb.setBoard(board);
						board[this.getColumn()][this.getRow()] = colorBomb;
						return true; // exit the function
					}
			}
			
			// crush column:
			sum = countUp+countDown;
			switch (sum)
			{
				case 2: 
					// 3 of a kind
					{
						crushColorIncol(countUp , countDown);
						return true; // exit the function
					}
				case 3:
					// 4 of a kind
					// vertical striped bags
					{
						crushColorIncol(countUp , countDown);
						VerticalStripedCandy newVerticallStriped = new VerticalStripedCandy(thisColor*10 + 1);
						newVerticallStriped.setColumn(this.getColumn());
						newVerticallStriped.setRow(this.getRow());
						newVerticallStriped.setBoard(board);
						newVerticallStriped.setCandyIcon(Candy_Bank.getIcon(thisColor));
						board[this.getColumn()][this.getRow()] = newVerticallStriped;
						return true; // exit the function
					}
				case 4: 
					// 5 of a kind
					// color bomb
					{
						crushColorIncol(countUp , countDown);
						BombCandy colorBomb = new BombCandy(10);
						colorBomb.setRow(this.getRow());
						colorBomb.setColumn(this.getColumn());
						colorBomb.setBoard(board);
						board[this.getColumn()][this.getRow()] = colorBomb;
						return true; // exit the function
					}
			}
		}
		return false;
	}
	
	
	private void crushColorIncol(int countUp, int countDown )  
	// the function crush candies in the column 
	{
		for (int i = countUp ; i>0 ; i--)
		{
			board[this.getColumn()][this.getRow()-i].crush();
		}
		for (int i = countDown ; i>0 ; i--)
		{
			board[this.getColumn()][this.getRow()+i].crush();
		}
		this.crush();
	}

	private void crushColorInRow(int countLeft, int countRight ) 
	// the function crush candies in the row 
	{
		this.crush();
		for (int i = countLeft ; i>0 ; i--)
		{
			board[this.getColumn()-i][this.getRow()].crush();
		}
		for (int i = countRight ; i>0 ; i--)
		{
			board[this.getColumn()+i][this.getRow()].crush();
		}
	}
	
	public void combine (Candy other) {
		other.accept(this);
	}

	
	public Candy getPrev() {
		if (this.getColumn() > 0 && board!=null)
		{
			return board[this.getColumn()-1][this.getRow()];
		}
		return null;
	}


	public Candy[][] getBoard() {
		return board;
	}

	public void setBoard(Candy[][] board) {
		this.board = board;
	}

	public Candy getUp() {
		if (this.getColumn() > 0 && board!=null)
		{
			return board[this.getColumn()-1][this.getRow()];
		}
		return null;
	}


	public Candy getNext() {
		if (this.getColumn() < SIZE && board!=null)
		{
			return board[this.getColumn()+1][this.getRow()];
		}
		return null;
	}


	public Candy getDown() {
		if (this.getRow() < SIZE-1 && board!=null)
		{
			return board[this.getColumn()][this.getRow()+1];
		}
		return null;
	}


	public int getColumn() {
		return column;
	}

	public void setColumn(int column) {
		this.column = column;
	}

	public int getRow() {
		return row;
	}

	public void setRow(int row) {
		this.row = row;
	}

	public int getColor() {
		return color;
	}

	public void setColor(int color) {
		this.color = color;
	}
	

	public void candyDown(Candy[][] board2, int col , int row)
	// the function make the candy fall until it hit another candy
	// or the board borders
	{
		board= board2;
		if (row < SIZE-1 && board[col][row+1].color==0)
			{
				Candy temp = this;
				int tempColor = this.getColor();
				board[col][row].setColor(0);
				Candy down = board[col][row+1];
				down.setBoard(board);
				int rowTemp = row;
				while (down!=null && rowTemp < SIZE-1 &&  down.getColor()==0)
				{ 
					down=down.getDown();
					if (down!=null)
					{
						down.setBoard(board);
					}
					
					rowTemp++;
				}
				board[col][rowTemp]=temp;
				temp.setColor(tempColor);
				board[col][row]= new RegularCandy(0) ;
				this.setColumn(col);
				this.setRow(row);
			}
		
		
	}

	public ImageIcon getCandyIcon() {
		return candyIcon;
	}

	public void setCandyIcon(ImageIcon candyIcon) {
		this.candyIcon = candyIcon;
	}
	
}
