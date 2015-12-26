import javax.swing.ImageIcon;


public class HorizontalStripedCandy extends Candy {

	public HorizontalStripedCandy(int color) {
		super(color);
		
	}

	public HorizontalStripedCandy(int color, ImageIcon candyIcon) {
		super(color, candyIcon);
	}



	@Override
	public void accept(visitor v) {
		v.visit(this);
	}

	@Override
	public void crush() 
	{
		if (this.color != 0)
		{	
			this.color = 0;
			this.setCandyIcon(Candy_Bank.crushed);
			for (int col = 0 ; col <= SIZE-1 ; col++)
			{
				if (this.board[col][this.row] != this & this.board[col][this.row].getColor() != 0)
				{
					this.board[col][this.row].crush();
				}
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
		other.setColor(0);
		other.setCandyIcon(null);
		for (int row = 0 ; row <= SIZE-1 ; row++)
		{
			if (this.board[other.getColumn()][row]!= other)
			{
				this.board[other.getColumn()][row].crush();
			}
		}
		
		if (this.row != other.getRow())
			this.crush();
		
	}

	@Override
	public void visit(VerticalStripedCandy other) 
	{
		this.board[other.getColumn()][other.getRow()] = this;
		this.board[this.column][this.row] = other;
		this.crush();
		if (this.column != other.getColumn())//if other was in the same column, other already exploded
		{
			other.crush();
		}
		
	}

	@Override
	public void visit(BagCandy other) 
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
	public void visit(BombCandy other) 
	{
		int color = this.color/10;
		this.board[other.getColumn()][other.getRow()] = new RegularCandy(0);
		this.board[column][row] = new RegularCandy(0);
		for (int i = 0; i <= SIZE - 1; i++)
			for (int j = 0; j <= SIZE - 1; j++)
			{
				if (this.board[i][j].getColor() == color)
				{
					if (Math.random() <= 0.5)
						this.board[i][j] = new HorizontalStripedCandy(color * 10 + 2);
					else 
						this.board[i][j] = new VerticalStripedCandy(color * 10 + 1);
					
					
					this.board[i][j].setBoard(this.board);
				}
			}
		
		for (int i = 0; i <= SIZE - 1; i++)
			for (int j = 0; j <= SIZE - 1; j++)
			{
				if ((this.board[i][j].getColor() == color * 10 + 2) | (this.board[i][j].getColor() == color * 10 + 1))
				{
						this.board[i][j].crush();
				}
			}
		
	}


}
