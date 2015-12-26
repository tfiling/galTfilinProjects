import javax.swing.ImageIcon;


public class BombCandy extends Candy {

	public BombCandy(int color) {
		super(color);
	}

	public BombCandy(int color, ImageIcon candyIcon) {
		super(color, candyIcon);
	}


	@Override
	public void accept(visitor v) {
		v.visit(this);
	}

	@Override
	public void crush() 
	{//it was crushed on its own, so a random color is chosen to be crushed
		
		this.board[column][row] = new RegularCandy(0);
		this.setCandyIcon(Candy_Bank.crushed);
		
		int randomColor = (int)((Math.random()*6)+1);//the random color that will be crushed
		
		for (int i = 0; i <= SIZE - 1; i++)
			for (int j = 0; j <= SIZE - 1; j++)
			{//crushes all of the candies in this color
				if (this.board[i][j].getColor() == randomColor)
				{
					this.board[i][j].crush();
				}				
			}

	}

	@Override
	public void visit(RegularCandy other) 
	{
		int crushedColor = other.getColor();
		this.board[this.column][this.row] = new RegularCandy(0);
		this.board[other.getColumn()][other.getRow()].crush();
		for (int i = 0; i <= SIZE - 1; i++)
			for (int j = 0; j <= SIZE - 1; j++)
			{
				if (this.board[i][j].getColor() == crushedColor)
				{
					this.board[i][j].crush();
				}
			}
		
	}

	@Override
	public void visit(HorizontalStripedCandy other) 
	{
		int color = other.getColor()/10;
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
	
	@Override
	public void visit(VerticalStripedCandy other) 
	{
		int color = other.getColor()/10;
		this.board[other.getColumn()][other.getRow()] = new RegularCandy(0);
		this.board[column][row] = new RegularCandy(0);
		for (int i = 0; i <= SIZE - 1; i++)
			for (int j = 0; j <= SIZE - 1; j++)
			{
				if (this.board[i][j].getColor() == color)
				{
					if (Math.random() <= 0.5)
					{
						this.board[i][j] = new HorizontalStripedCandy(color * 10 + 2);
					}
					else
					{
						this.board[i][j] = new VerticalStripedCandy(color * 10 + 1);
					}
					
					
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
	@Override
	public void visit(BagCandy other) 
	{
		int color = other.getColor() / 10;
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
	@Override
	public void visit(BombCandy other) 
	{
		this.board[this.column][this.row] = new RegularCandy(0);
		this.board[other.getColumn()][other.getRow()] = new RegularCandy(0);
		
		for (int i = 0; i <= SIZE - 1; i++)
			for (int j = 0; j<= SIZE - 1; j++)
			{
				this.board[i][j].crush();
			}
		
	}

}
