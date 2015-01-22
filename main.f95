program polynomialCalc
implicit none
    character(Len=100):: polynomialString="fdf"
    character(Len=100):: tmp
    
    call RemoveSpaces(polynomialString, tmp)

    Print *, tmp
    Print *, Len(Trim(tmp))
end program polynomialCalc

Subroutine RemoveSpaces(polynomialString, result)
    character(Len=100), intent(in):: polynomialString
    character(Len=100), intent(out):: result

    result=polynomialString
    
    Return
END