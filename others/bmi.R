library(tidyverse)
library(data.table)

#China cohort survey data
DT <- fread("D:\\OneDrive\\GitHub\\app-WELL_prototype\\others\\China_MetS.csv")
nrow(DT)

#Bmi categories
DT[bmi < 18.5, bmi_c := 1]
DT[bmi >= 18.5 & bmi < 23, bmi_c := 2]
DT[bmi >= 23 & bmi < 25, bmi_c := 3]
DT[bmi >= 25 & bmi < 27, bmi_c := 4]
DT[bmi >= 27, bmi_c := 5]

DT[!is.na(bmi_c)] %>% nrow()
DT[bmi_c == 5] %>% nrow()


"
------------------------------------------------------------
Tabulate by bmi categories
------------------------------------------------------------
"
#--Raised triglycerides
#>= 1.7 (tg)
DT[, rt := tg >= 1.7]
DT[, sum(rt, na.rm = TRUE), bmi_c]
DT[rt == TRUE] %>% nrow()
DT[1, hdl]


#--Reduced HDL cholesterol
#Male: < 1.03; female: < 1.29 (gender_it 1, 2; hdl)
DT[, rhc := (gender_it == 1 & hdl < 1.03) | (gender_it == 2 & hdl < 1.29)]
DT[, sum(rhc, na.rm = TRUE), bmi_c]
DT[rhc == TRUE] %>% nrow()
DT[hdl == 1.03, hdl]
DT[gender_it == 1 & hdl < 1.03] %>% nrow()
DT[gender_it == 2 & hdl < 1.29] %>% nrow()


#--Raised blood pressure
#Systolic BP >= 130 (sbp)
#Diastolic >= 85 (dbp)
DT[, rbp := sbp >= 130 | dbp >= 85]
DT[, sum(rbp, na.rm = TRUE), bmi_c]


#--Raised fasting plasma glucose
#FPG >= 5.6 (fpg)
#Previously diagnosed type 2 (t2dm==1)
DT[, rfpg := fpg >= 5.6 | t2dm == 1]
DT[, sum(rfpg, na.rm = TRUE), bmi_c]


#--Metabolic syndrome
#Male: waist >= 90; female: >= 80 (gender_it 1, 2; wc)
#BMI > 30kg/m2 (bmi)
#Plus any of the 2 following factors
DT[, ms := (bmi > 30) | (((gender_it == 1 & wc >= 90) | (gender_it == 2 & wc >= 80)) & (rt + rhc + rbp + rfpg >= 2))]
DT[, sum(ms, na.rm = TRUE), bmi_c]
DT[ms == TRUE] %>% nrow()
