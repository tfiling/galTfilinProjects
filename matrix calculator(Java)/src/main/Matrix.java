package main;
import java.util.*;


public class Matrix {
	
	//fields:
	private Vector<MathVector> rows;
	private int variables;	//the amount of variables in the mathVectors inside this matrix
	private boolean complex;	//true indicates that the equations are in the complex field
	
	//constructors:
	public Matrix()
	{
		this.rows = new Vector<MathVector>();
	}
	
	public Matrix(int variables, boolean complex,Vector<MathVector> rows) {
		this.rows = rows;
		this.complex = complex;
		this.variables = variables;
	}
	
	public Matrix(int variables, boolean complex){
		this.rows = new Vector<MathVector>();
		this.complex = complex;
		this.variables = variables;
	}
	
	
	// getters and setters:
	
	//returns the equation in the i'th row
	public MathVector getRows(int i) {
		return this.rows.elementAt(i);
	}

	public void setRows(Vector<MathVector> rows) {
		// this function set the rows of the matrix
		this.complex = rows.get(0).getType();
		this.variables = rows.get(0).getN();
		this.rows = rows;
	}
		
	public void add(MathVector v1)
	// this function add a row of scalars to the matrix
	{
		if (this.rows.size() == 0)
		{
			this.variables = v1.getN();
			this.complex = v1.getType();
			this.rows.add(v1);
		}			
		else if (v1.getN() == this.variables & this.complex == v1.getType())
			this.rows.add(v1);
	}
	
	public Vector<MathVector> getRows() {
		return rows;
	}
	
	public boolean getType()
	{
		return this.complex;
	}
	
	public int getVariables()
	{
		return this.variables;
	}
	
	//sets the equation in the i'th row
	public void setRows(MathVector v1, int i) {
		if (i <= this.rows.size() & this.complex == v1.getType())
			this.rows.setElementAt(v1, i);
	}
	
	
	// methods:	
		
	public Matrix add(Matrix mat)
	//returns a matrix which is the sum of mat+this
	{
		Matrix result = new Matrix(this.variables,this.complex) ;
		if (mat.rows.size() == this.rows.size() &
				this.variables == mat.getVariables() &
				this.complex == mat.getType())
		{
			for (int i=0 ; i<this.rows.size() ; i++)
			{
				result.rows.add(this.rows.elementAt(i).add(mat.rows.elementAt(i)));
			}
		}
		else 
		{
			System.out.println("the two matrix that were inserted were not valid for the chosen calculation");
			return null;  
		}
			
		result.minimize();
		return result;
	}

	public Matrix mul(Matrix mat)
	{		
		if ( mat.rows.size() != this.rows.firstElement().getN() |
				this.complex != mat.getType())
		{
			System.out.println("the two matrix that were inserted were not valid for the chosen calculation");
			return null;				
		}
		Matrix result = new Matrix(mat.getVariables(), this.complex);
		Matrix otherMat = mat.transpose();
		for (int i = 0; i < this.rows.size(); i++)
		{
			LinkedList<Scalar> currentVectorValues = new LinkedList<Scalar>();
			for(int j = 0; j < otherMat.rows.size(); j++)
			{
				currentVectorValues.add(this.rows.elementAt(i).mul(otherMat.rows.elementAt(j)));
			}
			MathVector newRowforResult = new MathVector(currentVectorValues.size(),  this.complex, currentVectorValues);
			result.add(newRowforResult);
		}
		result.minimize();
		return result;
	}
	
	public void rowSwitching(int i, int j)
	// the function switch rows i and j
	{
		MathVector temp = new MathVector(this.rows.firstElement().getN());
		temp = this.rows.elementAt(i);
		this.rows.set(i, this.rows.elementAt(j));
		this.rows.set(j, temp);
	}
	
	public Matrix transpose()
	{
		Matrix result = new Matrix(this.rows.size(), this.complex);
		for (int i = 0; i < this.variables; i++)
		{
			LinkedList<Scalar> nextRowValues = new LinkedList<Scalar>();
			for(int j = 0; j < this.rows.size(); j++)
			{
				nextRowValues.add(this.rows.get(j).getValues().get(i));
			}
			MathVector nextRow = new MathVector(this.rows.size(), this.complex, nextRowValues);
			result.add(nextRow);	
		}
		result.minimize();
		return result;
	}
	
	public Matrix Solve()
	// Gal -this isn't working the line below.
	{
		if (this.variables < this.rows.size())
		{
			System.out.println("this equasion is unsolvable:" + System.getProperty("line.separator") + System.getProperty("line.separator"));
			this.toString();
			return null;
		}
		else
		{
				return solveMat();
		}
	}
	


	private Matrix solveMat() {
		Matrix result = this.copyMat();
		if (result!=null ) 
		{
			//Stage 1 :
			//set the first row and scalar to begin :
			MathVector firstRow =this.getRows().firstElement();
			Scalar firstScalar = firstRow.getValues().get(0);
			Rational zero = new Rational(0, 1);
			// set the indexes of the matrix to begin with:
			int firstColIndex = 0;
			int firstRowIndex = 0;
			
			//check that the matrix begin with scalar different then zero:
			while (firstScalar.equalzero(zero))
			{	
				if (firstRowIndex!= result.getVariables()-2)
				{
					result.rowSwitching(0, firstRowIndex+1);
					firstRow = this.nextRow(firstRow);
					firstScalar = firstRow.getValues().get(0);
					firstRowIndex++;
				}
				else
				{
					if (firstColIndex!=result.getRows().capacity()-1)
					{
						firstColIndex++;
						firstRowIndex=0;
						firstRow = this.nextRow(firstRow);
						firstScalar = firstRow.getValues().get(firstColIndex);
					}
					else
					{
						System.out.println("there is no solution");
						return result;
					}
				}
			}
			// now the first scalar is different than 0
			MathVector VectoMultipy = new MathVector (result.rows.size(),this.complex);
			Rational neg = new Rational(-1,1);
			// the loops below make zero's from the diagonal down
			for ( int j = firstColIndex ; j< result.rows.size()-1 && result.thereIsResult() ; j++)
			{
				
				Scalar toMultiply1 = firstScalar;
				for ( int i = 1+j ; i< result.variables-1 && result.thereIsResult() ; i++)
				{
					// the loop multiply the rows with scalars and then substract them
					Scalar toMultiply2 = result.getRows(i+1).getValues().get(j);
					VectoMultipy = result.getRows(i+1);					
					MathVector copyOfFirstRow = firstRow.mulByScalar(toMultiply2);
					VectoMultipy = VectoMultipy.mulByScalar(toMultiply1);
					
					// substract the vector in the line below from the top line Vector.
					copyOfFirstRow = copyOfFirstRow.mulByScalar(neg);
					MathVector newVec= new MathVector(result.getRows().capacity(), result.complex);
					newVec = VectoMultipy.add(copyOfFirstRow);
					result.setRows(newVec, i+1);
				} 
				if (!thereIsResult()) // if there is a row with all 0'
				{ 
					System.out.println("There is no solution , there is a line of 0'");
					return result;
				}
				if (this.nextRow(firstRow)!=null)
				{
					firstRow=this.nextRow(firstRow);
					firstScalar = nextFirstScalar(firstColIndex, firstRowIndex, firstScalar);
					firstColIndex ++;
					firstRowIndex ++;
				}
				
			}
			if (!thereIsResult()) 
			{ 
				System.out.println("there is no solution , there is a line of 0'"); 
				return result;
			}
		
			// now from the diagonal down there are only zero's
			// solving: 
			
			for (int m = result.variables-1 ; m>=1 ;m--)
			// this loop makes the diagonal line to be "1".
			{
				MathVector lastRow = result.rows.get(m-1);
				Scalar toMultiplyTheRow = lastRow.getValues().get(m-1);
				toMultiplyTheRow = toMultiplyTheRow.inv();
				lastRow=lastRow.mulByScalar(toMultiplyTheRow);
				result.setRows(lastRow, m);
				lastRow= result.prevRow(lastRow);
			}
			
			// now the matrix is ranked and the diagonal is 1
			//now showing the answers:
			MathVector lastRow = result.rows.lastElement();
			MathVector beforeLastRow =  result.prevRow(lastRow);
			int howManyRows = result.variables-1;
			int indexScalarToSubstract = result.variables-1;
			int j=0;
			Scalar sca = beforeLastRow.getValues().get(indexScalarToSubstract-1);
			while (howManyRows>=1)
			{
				
				MathVector copyOfBeforeLastRow=beforeLastRow;
				
				while (!sca.equalzero(zero))
				{
					// Subtract the vector from the vector below
					MathVector  lastRowInNegative = lastRow.mulByScalar(sca);
					lastRowInNegative = lastRowInNegative.mulByScalar(neg);
					copyOfBeforeLastRow = copyOfBeforeLastRow.add(lastRowInNegative);
					result.setRows(copyOfBeforeLastRow, howManyRows-1);
					//  get the next scalar 
					sca = copyOfBeforeLastRow.getValues().get(indexScalarToSubstract-1);
				}
				howManyRows--;
				if (result.prevRow(copyOfBeforeLastRow)!=null) // if the before last row is the first
				{
					beforeLastRow =  result.prevRow(copyOfBeforeLastRow);
				}
				else 
				{
					j++;
					howManyRows = result.variables-1-j;;
					lastRow = result.prevRow(lastRow);
					if (result.prevRow(lastRow)!=null)
					{
						beforeLastRow =  result.prevRow(lastRow);
						indexScalarToSubstract--;
						sca = beforeLastRow.getValues().get(indexScalarToSubstract-1);	
						
					}
					else 
					{
						// need to get out of the loop
						howManyRows=0;
					}
				}
				sca = beforeLastRow.getValues().get(indexScalarToSubstract-1);
			}
		
			result.minimize();
	}
	return result;
}
	
	
private Scalar nextFirstScalar (int col , int row , Scalar first) 
{
	// the function returns the next scalar after the "first Scalar" , right down from it.
	if (col > this.getRows().size()- 1) 
		return first;
	Scalar result = this.getRows(row+2).getValues().get(col+1);
	return result;
	
}

private boolean thereIsResult ()
//the function checks that there is no row of zero's
{
	Rational zero = new Rational(0, 1);	
	if (this.complex)
	{
		int j = 0; 
		for (int i= 0 ; i <this.variables-1 ; i++)
		{
			if (this.getRows(i+1).getValues().get(j).equalzero(zero))
			{
				return false;
			}
			else 
				j++;
		}
	}
	else if (!this.complex)
	{
		int m = 0; 
		for (int i= 0 ; i <this.variables-1 ; i++)
		{
			if (this.getRows(i+1).getValues().get(m).equalzero(zero))
			{
				return false;
			}
			else 
				m++;
		}
		
	}
	return true;
}
	
private MathVector nextRow (MathVector prev)
// the function returns the row next to the given row, null if this is the last row
{
	if (this.rows.indexOf(prev)==this.rows.size())
		{
			return null;
		}
	int rowOfPrev = this.rows.indexOf(prev);
	if (rowOfPrev==this.rows.size()-1) 
		{
			return null;
		}
	return this.rows.elementAt(rowOfPrev+1);
}

private MathVector prevRow (MathVector next)
//the function returns the row prev to the given row, null if this is the first row
{
	boolean indexOfNext =this.rows.contains(next);
	if (!indexOfNext)
	{
		return null;
	}
	int indexOfNextint = this.rows.indexOf(next);
	if (indexOfNextint == 0)
		{
		return null; // first row
		}
	
	int rowOfnext = this.rows.indexOf(next)-1;
	return this.rows.elementAt(rowOfnext);
}


	public String toString()
	{
		String result = "";
		for (MathVector eq : this.rows)
		{
			result += eq.toString() + System.getProperty("line.separator");
		}
		return result;
	}
	
	
	private Matrix copyMat()
	// the function returns the same "this" matrix
	{
		Matrix ret= new Matrix( this.getVariables(), this.getType());
		ret.setRows(this.getRows());
		return ret;
	}

	
	private void minimize() 
	{
		for (MathVector v1: rows)
		{
			for (Scalar s1: v1.getValues())
			{
				s1.minimize();
			}
		}
	}
		
}
