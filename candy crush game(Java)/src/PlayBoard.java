import java.awt.GridLayout;

import javax.swing.ImageIcon;
import javax.swing.JLabel;
import javax.swing.JPanel;


public class PlayBoard extends JPanel {

	final static int SIZE = 9; 
	public Candy[][] board;
	public Board itsBoardLogics;
	public JLabel[][] lables;
	public boolean boardIsUpdate;
	
	public PlayBoard()
	{
		super(new GridLayout(SIZE,SIZE));
		removeAll();
		setSize(450, 450);
        setVisible(true);
 
        itsBoardLogics= new Board();
        board = itsBoardLogics.getBoard();
        lables = new JLabel[SIZE][SIZE];

        //arrange the labels on the screen
        for ( int i= 0 ; i < SIZE ; i++)
		{
			for (int j = 0 ; j < SIZE ; j++ )
			{
				lables[j][i]= new JLabel(board[j][i].getCandyIcon());
				add(lables[j][i]);
			}
		}

	}
	


	public static void main(String args[])
	{
		PlayBoard frame = new PlayBoard();
	}

	public void updateBoard()
	{
		this.removeAll();
		JLabel toAdd;
		ImageIcon icon;
		int color ;
		 for ( int col= 0 ; col < SIZE ; col++)
			{
				for (int row = 0 ; row < SIZE ; row++ )
				{
					color =this.board[col][row].getColor();
					icon = Candy_Bank.getIcon(color);
					this.board[col][row].setCandyIcon(icon);
					toAdd= new JLabel(icon);
					lables[col][row]=toAdd;
					
				}
			}
		 addlables();
	}
	
	private void addlables()
	{
		this.removeAll();
		 
		 for (int row = 0 ; row < SIZE ; row++ )
			{
				for  ( int col= 0 ; col < SIZE ; col++)
				{
					add(lables[col][row]);
				}
			}
		 repaint();
	}
	
	public int updateBoardAfterMove ()
	{
		int score = 0;
		score = itsBoardLogics.updateScoreAfterMove(this.board);
		int duplicateScore = 1;
		boolean boardchanged = (score > 0);
		while (boardchanged | itsBoardLogics.EmptyCells(this.board))
		{
			showCrushedCandies();
			while (itsBoardLogics.boardDown(this.board))
			{
				showEmptyCells();
			}
			
			boardchanged = (itsBoardLogics.playAlone(this.board));
			
			duplicateScore++;
			
			score = itsBoardLogics.updateScoreAfterMove(this.board)*duplicateScore + score;
		}
		

		
		showEmptyCells();
		
		return score;
		  
	}




	private void showEmptyCells() {
		
		this.removeAll();
		 
		 for (int row = 0 ; row < SIZE ; row++ )
			{
				for  ( int col= 0 ; col < SIZE ; col++)
				{

						int color = this.board[col][row].getColor();
						ImageIcon icon = Candy_Bank.getIcon(color);
						this.board[col][row].setCandyIcon(icon);
						this.lables[col][row].setIcon(icon);
						add(this.lables[col][row]);
					
				}
			}
		 this.update(getGraphics());
		 try {
			Thread.sleep(300);
		} catch (InterruptedException e) {
		}
	}
	
	private void showCrushedCandies() {
		
		this.removeAll();
		 
		 for (int row = 0 ; row < SIZE ; row++ )
			{
				for  ( int col= 0 ; col < SIZE ; col++)
				{

						this.lables[col][row].setIcon(this.board[col][row].getCandyIcon());
						add(this.lables[col][row]);
					
				}
			}
		 this.update(getGraphics());
		 try {
			Thread.sleep(400);
		} catch (InterruptedException e) {
		}
	}



	public Candy[][] getBoard ()
	{
		return turnBoard(this.board);
			
	}
	
	private Candy[][] turnBoard (Candy[][] board)
	{
		Candy[][] toReturn = new Candy [SIZE][SIZE];
		for ( int col = 0 ; col<SIZE ; col++)
		{
			for  ( int row = 0 ; row <SIZE ; row++)
			{
				toReturn[col][row]= board[row][col];
				toReturn[col][row].setColumn(col);
				toReturn[col][row].setRow(row);
				toReturn[col][row].setBoard(toReturn);
				
			}
		}
		return toReturn;
	}

	
	public void setBoard(Candy[][] boardNew)
	{
		 this.board =  boardNew;
	}

}
