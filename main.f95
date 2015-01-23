Program polynomialCalc
    character(Len=100):: poly1="5 + 2x^ 2"
    character(Len=100):: poly1WithoutSpaces=""
    integer:: degree
    
    Call RemoveSpaces(poly1, poly1WithoutSpaces)
    Call FindDegree(poly1WithoutSpaces, degree)

    Print*, poly1WithoutSpaces
    Print*, degree
END Program polynomialCalc

Subroutine RemoveSpaces(polynomialString, result)
    character(Len=100), intent(in):: polynomialString
    character(Len=100), intent(out):: result
    integer:: resultIndex=0
    
    Do i=1, Len(Trim(polynomialString))
        If (polynomialString(i: i)/=" ") Then
            resultIndex=resultIndex+1;
            result(resultIndex: resultIndex)=polynomialString(i: i)
        Endif            
    END Do
    
    Return
END

Subroutine FindDegree(polynomialString, degree)
    character(Len=100), intent(in):: polynomialString
    integer, intent(out):: degree
    integer:: w
    character(Len=100) tmp
    degree=0
    
    Do i=1, Len(Trim(polynomialString))
        If (polynomialString(i: i)=="x") Then
            If (polynomialString(i+1: i+1)=="^") Then
                Read(polynomialString(i+2: i+2), *) w
            ELSE
                w=1
            Endif
                          
            If (w>degree) Then
                degree=w
            Endif                
        Endif  
    END Do
    
    Return
END