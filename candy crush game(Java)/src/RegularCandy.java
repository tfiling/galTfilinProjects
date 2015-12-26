import javax.swing.ImageIcon;


public class RegularCandy extends Candy {

	// attributes:
	// by super - Candy

	// methods:
	
	//constructor:
	public RegularCandy(int color)
	// candy with color 0 represent an empty tile
	// 1 ,2 , 3, 4, 5, 6 - the regular candies's color
	{
		super(color);
		
	}
	
	public RegularCandy(int color, ImageIcon candyIcon)
	{
		super(color);
		this.setCandyIcon(candyIcon);
	}

	public void crush()
	{
		this.color = 0;
		// 0 means no candy = empty tile in the board
		this.setCandyIcon(Candy_Bank.crushed);
	}
	


	
	public void visit(RegularCandy other)
	{
		// implementation for 
		
		// RegularCandy.combine(RegularCandy);
		
		// assumption : combine with two candies is possible only if 
		// it will cause crush - so there is 3 of the same with this or other
		
		 
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
	public void accept(visitor v) {
		v.visit(this);
	}

	@Override
	public void visit(HorizontalStripedCandy other) 
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
	public void visit(VerticalStripedCandy other) 
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
	public void visit(BagCandy other) 
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
	public void visit(BombCandy other) 
	{
		int crushedColor = this.color;
		this.crush();
		this.board[other.getColumn()][other.getRow()] = new RegularCandy(0);
		for (int i = 0; i <= SIZE - 1; i++)
			for (int j = 0; j <= SIZE - 1; j++)
			{
				if (this.board[i][j].getColor() == crushedColor)
				{
					this.board[i][j].crush();
				}
			}
		
	}
	
	

	
}
