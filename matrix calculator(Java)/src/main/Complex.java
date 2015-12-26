package main;
public class Complex implements Scalar {
	// fields
	private Rational a;
	private Rational b;
	
	// constructors:
	public Complex()
	{
		this.a = new Rational();
		this.b = new Rational();
	}
	public Complex(Rational a , Rational b)
	{
		a.minimize();
		b.minimize();
		this.a = a;
		this.b = b;
	}
	public Complex(Complex a)
	{
		this(a.getA(), a.getB());	
	}
	
	// getters and setters: 
	public Rational getA() {
		return a;
	}
	public void setA(Rational a) {
		a.minimize();
		this.a = a;
	}
	public Rational getB() {
		return b;
	}
	public void setB(Rational b) {
		b.minimize();
		this.b = b;
	}

	
	@Override
	public void setValue(Scalar s) {
		if (s instanceof Complex)
		{
			a.minimize();
			b.minimize();
			this.a = (((Complex)s).getA());
			this.b = (((Complex)s).getB());
		}		
	}
	
	// methods:
	@Override
	public Scalar add(Scalar s) {
		// checks if the scalar is Rational or Complex
		Complex result = new Complex();
		if (s instanceof Complex)
		{
			// added a+bi + a'+b'i = (a+a') + (b+b')i
			result.a = (Rational)(this.a.add(((((Complex) s).getA()))));
			result.b = (Rational)(this.b.add(((((Complex) s).getB()))));
		}
		else if (s instanceof Rational)
		{
			// added only the real number  a+bi+ g = (a+g)+ bi
			Rational ansA = new Rational();
			ansA = (Rational) this.a.add(s);
			result.a = (ansA);
		}
		else
		{
			return null;
		}
		result.a.minimize();
		result.b.minimize();
		return result;
	}
	
	@Override
	public Scalar mul(Scalar s) {
		// checks if the scalar is Rational or Complex
		
		Complex result = null;	
		if (s instanceof Complex)
			{
			// if the scalar is zero
				if (this.getA().getA() == 0 && this.getB().getA() == 0)
				{
					Rational resultIsZero = new Rational(0, 1);//a rational object with the value zero
					result = new Complex(resultIsZero, resultIsZero); 
					return result;
				}
			 	else 
			 	{// if the scalar is one
			 		if (((Complex) s).a.getA() == ((Complex) s).a.getB() && 
		 				((Complex) s).b.getA() == 0) 
				 	{
			 			this.a.minimize();
			 			this.b.minimize();
				 		result = this;
				 	}
			 		else
			 		{
			 			Rational aMulATag = (Rational) this.a.mul(((Complex)s).getA());//a*a'
			 			Rational bMulBTag = (Rational) this.b.mul(((Complex)s).getB());//b*b'
			 			Rational resultA = (Rational) aMulATag.add(bMulBTag.neg());
			 			Rational aMulBTag = (Rational) this.a.mul(((Complex)s).getB());//a*b'
			 			Rational bMulATag = (Rational) this.b.mul(((Complex)s).getA());//b*a'
			 			Rational resultB = (Rational) aMulBTag.add(bMulATag);
			 			resultA.minimize();
			 			resultB.minimize();
			 			result = new Complex(resultA, resultB);
			 		}
			 	}
			}
			else if (s instanceof Rational)
			{
				// added only the real number  (a+bi) g = (a*g) + (b*g)i
				Rational ansA = (Rational) this.a.mul(s);
				Rational ansB = (Rational) this.b.mul(s);
				ansA.minimize();
				ansB.minimize();
				result = new Complex(ansA, ansB);
			}
			return result;	
	}
	@Override
	public Scalar neg() {
		// returns (a+bi)*(-1) = (-a-bi)
		Complex result = new Complex();
		result.a.setA(this.a.getA() * (-1));
		result.b.setB(this.b.getB() * (-1));
		return result;
	}
	
	@Override
	public Scalar inv() {
		if (this.a.getA() == 0 && this.b.getB() == 0)//if the result is x/0
		{
			return null;
		}
		Rational denominator = new Rational();
		Rational aSquare = (Rational) this.a.mul(this.a);//A^2
		Rational bSquare = (Rational) this.b.mul(this.b);//B^2
		try
		{
			denominator = (Rational) aSquare.add(bSquare);//the mutual denominator: (A^2 + B^2)
			Rational aAValueResult = new Rational(this.a.getA()*denominator.getB(), denominator.getA()*this.a.getB());//the result's a(Rational object)		
			Rational bAValueResult = new Rational(this.b.getA()*denominator.getB(),  denominator.getA()*this.b.getB());//the result's b(Rational object)	
			
			aAValueResult.minimize();
			bAValueResult.minimize();

			Complex result = new Complex(aAValueResult, (Rational) bAValueResult.neg());
			return result;
		}
		catch (ArithmeticException e)
		{
			return null;
		}
	}
	
	
	public boolean equalzero (Rational zero)
	{
		if (zero instanceof Rational)
		{
			if (  ((Rational)zero) == this.a && ((Rational) zero) == this.b )
				// both a and b of "this" are 0
			{
				return true;
			}
			
		}
		return false;
		
	}
	
	public String toString()
	{
		return (this.a.toString() + "+" + this.b.toString() + "i");
	}
	
	public String toStringValue()
	{
		return (this.a + " + " + this.b + " *i");
	}

	public void minimize()
	{
		this.a.minimize();
		this.b.minimize();
	}

	
}
