import javax.swing.ImageIcon;


public class BagCandy extends Candy {

	public BagCandy(int color) {
		super(color);
	}

	public BagCandy(int color, ImageIcon candyIcon) {
		super(color, candyIcon);
	}


	@Override
	public void accept(visitor v) {
		v.visit(this);

	}

	@Override
	public void crush() {
		int row = this.row;
		int col = this.column;
		if (this.color != 0)
		{
			this.color = 0;
			this.setCandyIcon(Candy_Bank.crushed);
			
			// need to crush :
			//board[col-1][row-1] board[col+1][row-1]
			//board[col-1][row+1] board[col+1][row+1]
			//board[col][row+1] board[col][row-1]
			//board[col-1][row] board[col+1][row]
			
			if (col>0)
			{
				if (row>0)
				{
					board[col-1][row-1].crush();
				}
				if (row<SIZE-1)
				{
					board[col-1][row+1].crush();
				}
				board[col-1][row].crush();
			}
			if (col <SIZE-1)
			{
				if (row>0)
				{
					 board[col+1][row-1].crush();
				}
				if (row<SIZE-1)
				{
					board[col+1][row+1].crush();
				}
				board[col+1][row].crush();
			}
			if (row>0)
			{
				board[col][row-1].crush();
			}
			if (row<SIZE-1)
			{
				board[col][row+1].crush();
			}
		}

	}

	@Override
	public void visit(RegularCandy other) 
	{
		 if (other.getColor() == 0 || this.color == 0 ) // Illegal move
			  return;
		 
		this.board[other.getColumn()][other.getRow()] = this;
		this.board[this.column][this.row] = other;
		int originalRow = this.row;
		int originalColumn = this.column;
		this.row = other.getRow();
		this.column = other.getColumn();
		if (!this.crushNearcandy())
		{//there were no explosions so we restore the original row,column values
			this.row = originalRow;
			this.column = originalColumn;
		}		
	}

	@Override
	public void visit(HorizontalStripedCandy other) 
	{
		this.board[other.getColumn()][other.getRow()] = new RegularCandy(0);
		this.board[this.column][this.row] = new RegularCandy(0);
		if (this.column > 0)
		{
			for (int row = 0 ; row <= SIZE-1 ; row++)
			{
				if (this.board[this.column - 1][row]!= this)
				{
					this.board[this.column - 1][row].crush();
				}
			}
		}
		
		for (int row = 0 ; row <= SIZE-1 ; row++)
		{
			if (this.board[this.getColumn()][row]!= this)
			{
				this.board[this.getColumn()][row].crush();
			}
		}
		
		if (this.column < SIZE - 1)
		{
			for (int row = 0 ; row <= SIZE-1 ; row++)
			{
				if (this.board[this.column + 1][row]!= this)
				{
					this.board[this.column + 1][row].crush();
				}
			}
		}
		
		
		///////////////////////////////////////////////////////
		
		
		if (this.row > 0)
		{
			for (int col = 0 ; col <= SIZE-1 ; col++)
			{
				if (this.board[col][this.row - 1]!= this)
				{
					this.board[col][this.row - 1].crush();
				}
			}
		}
		
		for (int col = 0 ; col <= SIZE-1 ; col++)
		{
			if (this.board[col][this.row]!= this)
			{
				this.board[col][this.row].crush();
			}
		}
		
		if (this.row < SIZE - 1)
		{
			for (int col = 0 ; col <= SIZE-1 ; col++)
			{
				if (this.board[col][this.row + 1]!= this)
				{
					this.board[col][this.row + 1].crush();
				}
			}
		}
		
	}

	@Override
	public void visit(VerticalStripedCandy other) 
	{
		this.board[other.getColumn()][other.getRow()] = new RegularCandy(0);
		this.board[this.column][this.row] = new RegularCandy(0);
		if (this.column > 0)
		{
			for (int row = 0 ; row <= SIZE-1 ; row++)
			{
				if (this.board[this.column - 1][row]!= this)
				{
					this.board[this.column - 1][row].crush();
				}
			}
		}
		
		for (int row = 0 ; row <= SIZE-1 ; row++)
		{
			if (this.board[this.getColumn()][row]!= this)
			{
				this.board[this.getColumn()][row].crush();
			}
		}
		
		if (this.column < SIZE - 1)
		{
			for (int row = 0 ; row <= SIZE-1 ; row++)
			{
				if (this.board[this.column + 1][row]!= this)
				{
					this.board[this.column + 1][row].crush();
				}
			}
		}
		
		
		///////////////////////////////////////////////////////
		
		
		if (this.row > 0)
		{
			for (int col = 0 ; col <= SIZE-1 ; col++)
			{
				if (this.board[col][this.row - 1]!= this)
				{
					this.board[col][this.row - 1].crush();
				}
			}
		}
		
		for (int col = 0 ; col <= SIZE-1 ; col++)
		{
			if (this.board[col][this.row]!= this)
			{
				this.board[col][this.row].crush();
			}
		}
		
		if (this.row < SIZE - 1)
		{
			for (int col = 0 ; col <= SIZE-1 ; col++)
			{
				if (this.board[col][this.row + 1]!= this)
				{
					this.board[col][this.row + 1].crush();
				}
			}
		}
		
	}

	@Override
	public void visit(BagCandy bagCandy) {
		// Auto-generated method stub
		
	}

	@Override
	public void visit(BombCandy other) 
	{
		int color = this.color / 10;
		this.board[other.getColumn()][other.getRow()] = new RegularCandy(0);
		this.board[this.column][this.row] = new RegularCandy(0);
		for (int i = 0; i <= SIZE - 1; i++)
			for (int j = 0; j <= SIZE - 1; j++)
			{//crushes all of the candies in this color
				if (this.board[i][j].getColor() == color)
				{
					this.board[i][j].crush();
				}				
			}
		
		int randomColor = (int)((Math.random()*6)+1);//the additional random color that will be crushed
		while (randomColor == color)
		{
			randomColor = (int)((Math.random()*6)+1);
		}
		
		for (int i = 0; i <= SIZE - 1; i++)
			for (int j = 0; j <= SIZE - 1; j++)
			{//crushes all of the candies in this color
				if (this.board[i][j].getColor() == randomColor)
				{
					this.board[i][j].crush();
				}				
			}
		
	}

}
