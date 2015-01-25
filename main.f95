PROGRAM polynomialCalc
    CHARACTER(LEN=100):: poly1, poly2, poly3
    INTEGER:: deg1, deg2, deg3
    INTEGER, DIMENSION(:), ALLOCATABLE:: coefficients1
    INTEGER, DIMENSION(:), ALLOCATABLE:: coefficients2
    INTEGER, DIMENSION(:), ALLOCATABLE:: coefficients3

    poly1="x^4+4x^2-x+2"
    poly2="x^4+x^3+10"

    CALL RemoveSpaces(poly1)
    CALL FindDegree(poly1, deg1)
    ALLOCATE(coefficients1(deg1+1))
    CALL AnalyzePolynomial(poly1, deg1, coefficients1)
    CALL RemoveSpaces(poly2)
    CALL FindDegree(poly2, deg2)
    ALLOCATE(coefficients2(deg2+1))
    CALL AnalyzePolynomial(poly2, deg2, coefficients2)

    CALL DisplayPolynomial(deg1, coefficients1)
    PRINT *, ""
    CALL DisplayPolynomial(deg2, coefficients2)
    PRINT *, ""

    !------------------------------------------

    deg3=Max(deg1, deg2)

    IF (Allocated(coefficients3)) THEN
        DEALLOCATE(coefficients3)
    END IF

    ALLOCATE(coefficients3(deg3+1))
    CALL Add(deg1, coefficients1, deg2, coefficients2, deg3, coefficients3)
    PRINT *, ""
    CALL DisplayPolynomial(deg3, coefficients3)

    !------------------------------------------

    deg3=Max(deg1, deg2)

    IF (Allocated(coefficients3)) THEN
        DEALLOCATE(coefficients3)
    END IF

    ALLOCATE(coefficients3(deg3+1))
    CALL Subtract(deg1, coefficients1, deg2, coefficients2, deg3, coefficients3)
    PRINT *, ""
    CALL DisplayPolynomial(deg3, coefficients3)

    !------------------------------------------

    deg3=deg1+deg2

    IF (Allocated(coefficients3)) THEN
        DEALLOCATE(coefficients3)
    END IF

    ALLOCATE(coefficients3(deg3+1))
    CALL Multiply(deg1, coefficients1, deg2, coefficients2, deg3, coefficients3)
    PRINT *, ""
    CALL DisplayPolynomial(deg3, coefficients3)

    DEALLOCATE(coefficients1)
    DEALLOCATE(coefficients2)
    DEALLOCATE(coefficients3)
END PROGRAM polynomialCalc

SUBROUTINE RemoveSpaces(polynomialString)
    CHARACTER(LEN=100), INTENT(INOUT):: polynomialString
    CHARACTER(LEN=100):: stringWithoutSpaces
    INTEGER:: resultIndex
    INTEGER:: stringLength
    INTEGER:: removedChars
    resultIndex=0
    stringLength=LEN(Trim(polynomialString))
    removedChars=0

    DO i=1, stringLength
        IF (polynomialString(i: i)/=" ") THEN
            resultIndex=resultIndex+1;
            stringWithoutSpaces(resultIndex: resultIndex)=polynomialString(i: i)
        ELSE
            removedChars=removedChars+1
        ENDIF
    END DO

    DO i=stringLength+1-removedChars, LEN(polynomialString)
        stringWithoutSpaces(i:i)=" "
    END DO

    polynomialString=Trim(stringWithoutSpaces)
END

SUBROUTINE FindDegree(polynomialString, degree)
    CHARACTER(LEN=100), INTENT(IN):: polynomialString
    INTEGER, INTENT(OUT):: degree
    INTEGER:: w, dummy, stringLength, i
    i=1
    degree=0
    stringLength=LEN(Trim(polynomialString))

    DO
        IF (polynomialString(i: i)=="x") THEN
            i=i+1

            IF (polynomialString(i: i)=="^") THEN
                CALL ConvertStringToInt(polynomialString(i+1: stringLength), w, dummy)
            ELSE
                w=1
            ENDIF

            IF (w>degree) THEN
                degree=w
            ENDIF
        ELSE
            i=i+1
        ENDIF

        IF (i>stringLength) EXIT
    END DO
END

SUBROUTINE ConvertStringToInt(string, output, outputLength)
    CHARACTER(LEN=100), INTENT(IN):: string
    INTEGER, INTENT(OUT):: output
    INTEGER, INTENT(OUT):: outputLength
    INTEGER:: conversionFailed
    conversionFailed=1

    DO i=2, LEN(string)
        READ(string(1: i), *, IOSTAT=conversionFailed) output

        IF (conversionFailed/=0) THEN
            outputLength=i-1

            READ(string(1: outputLength), *) output
            EXIT
        ENDIF
    END DO
END

SUBROUTINE AnalyzePolynomial(polynomialString, degree, tableOfCoefficients)
    CHARACTER(LEN=100), INTENT(IN):: polynomialString
    INTEGER, INTENT(IN):: degree
    INTEGER, DIMENSION(degree+1), INTENT(OUT):: tableOfCoefficients
    INTEGER:: factor, power, coefficient, stringLength, displacement, i
    i=1
    stringLength=LEN(Trim(polynomialString))

    DO j=1, degree+1
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
            CALL ConvertStringToInt(polynomialString(i:stringLength), coefficient, displacement)

            i=i+displacement
        END IF

        IF (polynomialString(i:i)=="x") THEN
            i=i+1

            IF (polynomialString(i:i)=="^") THEN
                CALL ConvertStringToInt(polynomialString(i+1: stringLength), power, displacement)

                i=i+1+displacement
            ELSE
                power=1
            END IF
        END IF

        IF (power==0) THEN
            i=i+1
        END IF

        coefficient=coefficient*factor
        tableOfCoefficients(power+1)=tableOfCoefficients(power+1)+coefficient

        IF (i>stringLength) EXIT
    END DO
END

SUBROUTINE DisplayPolynomial(degree, tableOfCoefficients)
    INTEGER, INTENT(IN):: degree
    INTEGER, DIMENSION(degree+1), INTENT(IN):: tableOfCoefficients

    IF (degree==0) THEN
        PRINT *, tableOfCoefficients(1)
    ELSE
        DO i=degree+1, 1, -1
            IF (tableOfCoefficients(i)/=0) THEN
                IF (i/=degree+1 .AND. tableOfCoefficients(i)>0) THEN
                    WRITE(*, "(A)", ADVANCE="no") "+"
                END IF

                IF ((tableOfCoefficients(i)/=1 .OR. i==1) .AND. (tableOfCoefficients(i)/=-1 .OR. i==1)) THEN
                    WRITE(*, "(I0)", ADVANCE="no") tableOfCoefficients(i)
                ENDIF

                IF (tableOfCoefficients(i)==-1 .AND. i/=1) THEN
                    WRITE(*, "(A)", ADVANCE="no") "-"
                END IF

                IF (i/=1) THEN
                    WRITE(*, "(A)", ADVANCE="no") "x"
                END IF

                IF (i/=2 .AND. i/=1) THEN
                    WRITE(*, "(A)", ADVANCE="no") "^"
                    WRITE(*, "(I0)", ADVANCE="no") i-1
                END IF
            END IF
        END DO
    END IF
END

SUBROUTINE Add(degree1, tableOfCoefficients1, degree2, tableOfCoefficients2, resultDegree, tableOfCoefficientsOfResult)
    INTEGER, INTENT(IN):: degree1
    INTEGER, DIMENSION(degree1+1), INTENT(IN):: tableOfCoefficients1
    INTEGER, INTENT(IN):: degree2
    INTEGER, DIMENSION(degree2+1), INTENT(IN):: tableOfCoefficients2
    INTEGER, INTENT(INOUT):: resultDegree
    INTEGER, DIMENSION(resultDegree+1), INTENT(OUT):: tableOfCoefficientsOfResult
    INTEGER:: component1, component2, j

    DO i=1, resultDegree+1
        component1=0
        component2=0

        IF (i<=degree1+1) THEN
            component1=tableOfCoefficients1(i)
        END IF

        IF (i<=degree2+1) THEN
            component2=tableOfCoefficients2(i)
        END IF

        tableOfCoefficientsOfResult(i)=component1+component2
    END DO

    j=resultDegree+1

    DO
        IF (tableOfCoefficientsOfResult(j)==0 .AND. j/=1) THEN
            j=j-1;
        ELSE
            EXIT
        END IF
    END DO

    resultDegree=j-1
END

SUBROUTINE Subtract(degree1, tableOfCoefficients1, degree2, tableOfCoefficients2, resultDegree, tableOfCoefficientsOfResult)
    INTEGER, INTENT(IN):: degree1
    INTEGER, DIMENSION(degree1+1), INTENT(IN):: tableOfCoefficients1
    INTEGER, INTENT(IN):: degree2
    INTEGER, DIMENSION(degree2+1), INTENT(IN):: tableOfCoefficients2
    INTEGER, INTENT(INOUT):: resultDegree
    INTEGER, DIMENSION(resultDegree+1), INTENT(OUT):: tableOfCoefficientsOfResult
    INTEGER, DIMENSION(degree2+1):: subtrahend

    DO i=1, Size(tableOfCoefficients2)
        subtrahend(i)=-1*tableOfCoefficients2(i)
    END DO

    CALL Add(degree1, tableOfCoefficients1, degree2, subtrahend, resultDegree, tableOfCoefficientsOfResult)
END

SUBROUTINE Multiply(degree1, tableOfCoefficients1, degree2, tableOfCoefficients2, resultDegree, tableOfCoefficientsOfResult)
    INTEGER, INTENT(IN):: degree1
    INTEGER, DIMENSION(degree1+1), INTENT(IN):: tableOfCoefficients1
    INTEGER, INTENT(IN):: degree2
    INTEGER, DIMENSION(degree2+1), INTENT(IN):: tableOfCoefficients2
    INTEGER, INTENT(INOUT):: resultDegree
    INTEGER, DIMENSION(resultDegree+1), INTENT(OUT):: tableOfCoefficientsOfResult
    INTEGER:: tmp

    DO i=1, Size(tableOfCoefficientsOfResult)
        tableOfCoefficientsOfResult(i)=0
    END DO

    IF ((degree1==0 .AND. tableOfCoefficients1(1)==0) .OR. (degree2==0 .AND. tableOfCoefficients2(1)==0)) THEN
        resultDegree=0
    ELSE
        DO i=degree1+1, 1, -1
            DO j=degree2+1, 1, -1
                tmp=tableOfCoefficients1(i)*tableOfCoefficients2(j)
                tableOfCoefficientsOfResult(i+j-1)=tableOfCoefficientsOfResult(i+j-1)+tmp
            END DO
        END DO
    END IF
END
