PROGRAM polynomialCalc
    IMPLICIT NONE
    CHARACTER(LEN=100):: poly1="2x^2+5x^3+4"
    CHARACTER(LEN=100):: tmp
    INTEGER:: deg1
    INTEGER, DIMENSION(:), ALLOCATABLE:: coefficients1

    CALL RemoveSpaces(poly1, tmp)
    CALL FindDegree(tmp, deg1)
    ALLOCATE(coefficients1(deg1+1))
    CALL AnalyzePolynomial(tmp, deg1, coefficients1)

    PRINT *, tmp
    PRINT *, "Degree: ", deg1
    PRINT *, "Coefficients: ", coefficients1(1)

END PROGRAM polynomialCalc

SUBROUTINE RemoveSpaces(polynomialString, stringWithoutSpaces)
    CHARACTER(LEN=100), INTENT(IN):: polynomialString
    CHARACTER(LEN=100), INTENT(OUT):: stringWithoutSpaces
    INTEGER:: resultIndex=0
    INTEGER:: stringLength
    stringLength=LEN(Trim(polynomialString))

    DO i=1, stringLength
        IF (polynomialString(i: i)/=" ") THEN
            resultIndex=resultIndex+1;
            stringWithoutSpaces(resultIndex: resultIndex)=polynomialString(i: i)
        ENDIF
    END DO

    DO i=stringLength+1, LEN(polynomialString)
        stringWithoutSpaces(i:i)=" "
    END DO

    stringWithoutSpaces=Trim(stringWithoutSpaces)
END

SUBROUTINE FindDegree(polynomialString, degree)
    CHARACTER(LEN=100), INTENT(IN):: polynomialString
    INTEGER, INTENT(OUT):: degree
    INTEGER:: w
    INTEGER:: stringLength
    degree=0
    stringLength=LEN(Trim(polynomialString))

    DO i=1, stringLength
        IF (polynomialString(i: i)=="x") THEN
            IF (polynomialString(i+1: i+1)=="^") THEN
                CALL ConvertStringToInt(polynomialString(i+2: stringLength), w)
            ELSE
                w=1
            ENDIF

            IF (w>degree) THEN
                degree=w
            ENDIF
        ENDIF
    END DO
END

SUBROUTINE ConvertStringToInt(string, output)
    CHARACTER(LEN=100), INTENT(IN):: string
    INTEGER, INTENT(OUT):: output
    INTEGER:: conversionFailed=1

    DO i=2, LEN(string)
        READ(string(1: i), *, IOSTAT=conversionFailed) output

        IF (conversionFailed/=0) THEN
            READ(string(1: i-1), *) output
            EXIT
        ENDIF
    END DO
END

SUBROUTINE AnalyzePolynomial(polynomialString, degree, tableOfCoefficients)
    CHARACTER(LEN=100), INTENT(IN):: polynomialString
    INTEGER, INTENT(IN):: degree
    INTEGER, DIMENSION(degree+1), INTENT(OUT):: tableOfCoefficients
    INTEGER:: factor
    INTEGER:: power
    INTEGER:: coefficient
    INTEGER:: i=1
    INTEGER:: stringLength
    stringLength=LEN(Trim(polynomialString))

    DO j=1, Size(tableOfCoefficients)
        tableOfCoefficients(j)=0
    END DO

    DO
        factor=1
        power=0

        IF (polynomialString(i:i)=="-") THEN
            factor=-1
            i=i+1
        END IF

        IF (polynomialString(i:i)=="+") THEN
            i=i+1;
        END IF

        IF (polynomialString(i:i)=="x") THEN
            coefficient=1
        ELSE
            CALL ConvertStringToInt(polynomialString(i:stringLength), coefficient)

            i=i+1
        END IF

        IF (polynomialString(i:i)=="x") THEN
            i=i+1

            IF (polynomialString(i:i)=="^") THEN
                CALL ConvertStringToInt(polynomialString(i+1: stringLength), power)

                i=i+2
            ELSE
                power=1
            END IF
        END IF

        coefficient=coefficient*factor
        tableOfCoefficients(power+1)=tableOfCoefficients(power+1)+coefficient

        IF (i>stringLength) EXIT
    END DO
END

SUBROUTINE DisplayPolynomial(degree, tableOfCoefficients)
    INTEGER, INTENT(IN):: degree
    INTEGER, DIMENSION(degree+1), INTENT(IN):: tableOfCoefficients

! TODO (Zawadzcy#1#): not implemented

END
