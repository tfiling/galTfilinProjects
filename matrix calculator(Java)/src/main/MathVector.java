package main;
import java.util.LinkedList;

public class MathVector {
	//fields:
	private int n;
	private LinkedList<Scalar> values;
	private boolean complex;	//true indicates that the equation is in the complex field
	
	// constructor:
	public MathVector(int n)
	{
		this.n = n;
		this.values = new LinkedList<Scalar>();
	}
	
	public MathVector(int n, boolean complex)
	{
		this.n = n;
		this.complex = complex;
		this.values = new LinkedList<Scalar>();
	}
	
	public MathVector(int n, boolean complex, LinkedList<Scalar> values)
	{
		this.n = n;
		this.complex = complex;
		this.values = new LinkedList<Scalar>(values);
	}
	
	public MathVector(MathVector rows) {
		this.complex = rows.getType();
		this.values = new LinkedList<Scalar>(rows.getValues());
		this.n = rows.getN();
	}

	//getters and setters: 
	public int getN() {
		return n;
	}
	public void setN(int n) {
		this.n = n;
	}
	public LinkedList<Scalar> getValues() {
		return values;
	}
	public void setValues(LinkedList<Scalar> values) {
		this.values = values;
	}
	public boolean getType()
	{
		return this.complex;
	}
	
	//methods:
	public MathVector add(MathVector v1)
	//returns {a,b,c...}+{d,e,f...} = {a+d, b+e , c+f .... } 
	{
		MathVector result = new MathVector(n,this.complex);
		if (v1.getN()==this.getN() & this.complex == v1.getType()){
			for (int i=0 ; i<this.getN() ; i=i+1 )
			{
				Scalar scaTobeAdd =this.values.get(i);
				Scalar scaToAdd = v1.values.get(i);
				
				result.values.add(scaToAdd.add(scaTobeAdd));
			}
		}
		else // v1 isn't of appropriate size or they are not in the same field
		{
			return null ;  
		}
		return result;
	}
	
	public MathVector mulByScalar(Scalar s)
	//returns {a,b,c...}*x = {a*x, b*x , c*x .... } 
	{
		MathVector result = new MathVector(n,this.complex);
		for (int i=0 ; i<this.getN() ; i=i+1 )
		{
			result.values.add(this.values.get(i).mul(s));
		}	
		return result;
	}
	
	
	public Scalar mul(MathVector v1)
	{
		if (v1.getN() != this.getN() | this.complex != v1.getType())
		{
			return null;
		}
		Scalar result;
		if (this.getType() == true)
			result = new Complex();
		else 
			result = new Rational();
		LinkedList<Scalar> otherVector = v1.getValues();
		for (int i = 0; (i < this.values.size()) & (i < otherVector.size()); i++)
		{
			result = result.add(this.values.get(i).mul(otherVector.get(i)));
		}
		return result;
	}
	
	
	public Scalar sum()
	// this function summarize the vector scalars
	{
		if (this.complex)
		{
			Rational reta = new Rational(0, 1);
			Rational retb = new Rational(0, 1);
			Complex ret = new Complex(reta,retb);
			for (int i =0 ; i< this.getN() ; i++)
			{
				ret.add(this.getValues().get(i));
			}
			return ret;
			
		}
		else 
			// rational numbers
		{
			Rational ret = new Rational(0, 1);
			for (int i =0 ; i <= n-1 ; i++)
			{
				Scalar toAdd= this.values.get(i);
				ret= (Rational) ret.add(toAdd);
			}
			return ret;
		}
		
	}
	
	public String toString()
	{
		String result = "";
		for (int i = 0; i < n-1; i++)
			result += this.values.get(i).toString() + " ";
		result += this.values.get(n-1).toString();
		return result;
	}
	
}
