library(tidyverse)
library(data.table)

DT <- fread("D:\\OneDrive\\GitHub\\app-WELL_prototype\\others\\China_MetS.csv")
nrow(DT)

DT[bmi >= 18.5 & bmi < 23, bmi_c := 1]
DT[bmi >= 23 & bmi < 25, bmi_c := 2]
DT[bmi >= 25 & bmi < 30, bmi_c := 3]
DT[bmi >= 30 & bmi < 34, bmi_c := 4]
DT[bmi >= 34, bmi_c := 5]
DT$bmi_c


#--Raised triglycerides
#>= 1.7 (tg)
DT[, rt := tg >= 1.7]


#--Reduced HDL cholesterol
#Male: < 1.03; female: < 1.29 (hdl)
DT[, rhc := (gender_it == 4 & hdl < 1.03) | (gender_it == 5 & hdl < 1.29)]


#--Raised blood pressure
#Systolic BP >= 130 (sbp)
#Diastolic >= 85 (dbp)
DT[, rbp := sbp >= 130 | dbp >= 85]


#--Raised fasting plasma glucose
#FPG >= 5.6 (fpg)
#Previously diagnosed type 2 (t2dm==1)
DT[, rfpg := fpg >= 5.6 | t2dm == 1]


#--Central obesity
#Male: waist >= 90; female: >= 80 (gender_it 4, 5; wc)
#BMI > 30kg/m2 (bmi)
#Plus any of the 2 following factors
DT[, co := (bmi > 30) | (((gender_it == 4 & wc >= 90) | (gender_it == 5 & wc >= 80)) & (rt + rhc + rbp + rfpg >= 2))]
