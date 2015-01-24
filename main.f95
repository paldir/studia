PROGRAM polynomialCalc
    CHARACTER(LEN=100):: poly1, poly2, tmp
    INTEGER:: deg1, deg2
    INTEGER, DIMENSION(:), ALLOCATABLE:: coefficients1
    INTEGER, DIMENSION(:), ALLOCATABLE:: coefficients2

    poly1="2x^2"
    poly2="11x"

    CALL RemoveSpaces(poly1, tmp)
    CALL FindDegree(tmp, deg1)
    ALLOCATE(coefficients1(deg1+1))
    CALL AnalyzePolynomial(tmp, deg1, coefficients1)
    CALL RemoveSpaces(poly2, tmp)
    CALL FindDegree(tmp, deg2)
    ALLOCATE(coefficients2(deg2+1))
    CALL AnalyzePolynomial(tmp, deg2, coefficients2)

    CALL DisplayPolynomial(deg1, coefficients1)
    PRINT *, ""
    CALL DisplayPolynomial(deg2, coefficients2)

    DEALLOCATE(coefficients1)
    DEALLOCATE(coefficients2)
END PROGRAM polynomialCalc

SUBROUTINE RemoveSpaces(polynomialString, stringWithoutSpaces)
    CHARACTER(LEN=100), INTENT(IN):: polynomialString
    CHARACTER(LEN=100), INTENT(OUT):: stringWithoutSpaces
    INTEGER:: resultIndex
    INTEGER:: stringLength
    resultIndex=0
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
                IF (i/=degree+1 .and. tableOfCoefficients(i)>0) THEN
                    WRITE(*, "(A)", ADVANCE="no") " + "
                END IF

                IF ((tableOfCoefficients(i)/=1 .or. i==1) .and. (tableOfCoefficients(i)/=1 .or. i==0)) THEN
                    WRITE(*, "(I0)", ADVANCE="no") tableOfCoefficients(i)
                ENDIF

                IF (tableOfCoefficients(i)==-1 .and. i/=1) THEN
                    WRITE(*, "(A)", ADVANCE="no") " - "
                END IF

                IF (i/=1) THEN
                    WRITE(*, "(A)", ADVANCE="no") "x"
                END IF

                IF (i/=2 .and. i/=1) THEN
                    WRITE(*, "(A)", ADVANCE="no") "^"
                    WRITE(*, "(I0)", ADVANCE="no") i-1
                END IF
            END IF
        END DO
    END IF
END
