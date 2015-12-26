package main;


public class Rational implements Scalar{
	// fields: 
	private int a;
	private int b;
	
	// constructors:
	public Rational()
	{
		this.a = 0;
		this.b = 1;
	}
	public Rational(int a, int b)
	{
		this.a = a;
		if (b == 0)
		{
			System.out.println("the value " + a + "/" + b + " was inserted, the denominator was changed to 1");
			b = 1;
		}
		this.b = b;
		this.minimize();
	}
	
	public Rational(Rational a)
	{
		this(a.getA(), a.getB());
	}
	
	// getters and setters: 
	public int getA()
	{
		return this.a;
	}
	
	public void setA(int a)
	{
		this.a = a;
	}
	
	public int getB()
	{
		return this.b;
	}

	public void setB(int b)
	{
		if (b == 0)
		{
			System.out.println("the value " + a + "/" + b + " was inserted, the denominator was changed to 1");
			b = 1;
		}
		this.b = b;
	}
	
	public double getValue()
	{
		return (this.a / this.b);
	}
	
	
	public void setValue(Scalar s)
	{
		if (s instanceof Rational)
		{
			this.a = (((Rational)s).getA());
			this.setB(((Rational)s).getB());
			this.minimize();
		}
		else System.out.println("error: a rational scalar recieved a complex value");
	}
	
	// methods:
	@Override
	public Scalar add(Scalar s) {
		if (s instanceof Rational)
		{
			Rational result = new Rational();
			result.setA(((this.a * ((Rational) s).getB()) + (((Rational)s).getA() * this.b)));
			result.setB(this.b * ((Rational)s).getB());
			this.minimize();
			return result;
		}
		else return null;
	}
	@Override
	public Scalar mul(Scalar s) {
		if (s instanceof Rational)
		{
			Rational result = new Rational();
			result.setA(((this.a * ((Rational) s).getA())));
			result.setB(this.b * ((Rational)s).getB());
			this.minimize();
			return result;
		}
		else return null;
	}
	@Override
	public Scalar neg() {
		Rational result = new Rational();
		result.setA(this.a * -1);
		result.setB(this.b);
		this.minimize();
		return result;
	}
	@Override
	public Scalar inv() {
		Rational result = new Rational();
		result.setA(this.b);
		result.setB(this.a);
		this.minimize();
		return result;
	}

	
	public String toString()
	{
		return (this.a + "/" + this.b);
	}
	
	public String toStringValue()
	{
		Double result = (double) (this.a / this.b);
		return (result.toString());
	}
	
	public boolean equalzero (Rational s)
	{
		if (s instanceof Rational)
		{
			if ((((Rational) s).getA()==this.a ))
			{
				return true;
			}
			
		}
		return false;	
	}
	
	public void minimize()
	{
		int gcd = findGcd(this.a, this.b);
		this.a = this.a / gcd;
		this.b = this.b / gcd;
	}
	
	private int  findGcd(int m, int n)
	{
			int r = m%n;
			while(r!=0)
			{
				m = n;
				n = r;
				r = m%n;
			}
			int gcd = n;
			return gcd;
	}
}


