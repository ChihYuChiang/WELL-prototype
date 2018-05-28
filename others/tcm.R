library(tidyverse)
library(data.table)


"
------------------------------------------------------------
Preprocessing
------------------------------------------------------------
"
DT <- fread("tcm.csv")

#Filter out obs with NA
DT_filtered <- DT[!is.na(rowSums(DT))]
nrow(DT_filtered)




"
------------------------------------------------------------
Items for each type
------------------------------------------------------------
"
base_Ping <- c(1, 4, 7, 34, 46, 47, 49)
base_QiD <- c(2, 3, 5, 6, 20, 27)
base_YangD <- c(18, 19, 28, 35, 36, 43, 50)
base_YinD <- c(16, 17, 24, 26, 39, 42)
base_Tan <- c(15, 25, 48, 51, 53)
base_Shi <- c(33, 37, 38, 41, 52, 54)
base_Xie <- c(14, 22, 23, 29, 31, 32, 45)
base_QiY <- c(8, 9, 10, 11, 12, 13, 40)
base_Te <- c(211, 212, 213, 214, 30, 44)

special_Ping <- c(1, 4, 7, 34, 46, 47, 49)
special_QiD <- c(5, 6, 20)
special_YangD <- c(18, 19, 28, 35, 36)
special_YinD <- c(17, 24)
special_Tan <- c(25, 51)
special_Shi <- c(33, 37, 41)
special_Xie <- c(14, 29, 31, 32)
special_QiY <- c(10, 12, 13)
special_Te <- 44




"
------------------------------------------------------------
Compute markers
------------------------------------------------------------
"
#--Tools used in the computation
selectCol <- function(DT, targetColumn) {
  dt <- select(DT, matches(sprintf("^tcmv16tcm_0?(%s)$", paste(targetColumn, collapse="|"))))
  return(dt)
}
DT_LTE3 <- as.data.table(DT_filtered >= 3)
DT_LTE4 <- as.data.table(DT_filtered >= 4)


#--QiD
baseM_QiD <- selectCol(DT_filtered, base_QiD) %>% rowMeans() / 5
specialLTE3_QiD <- selectCol(DT_LTE3, special_QiD) %>% rowSums()

DT_filtered[, b_QiD := specialLTE3_QiD >= 2 & baseM_QiD >= 0.6]
DT_filtered[, t_QiD := (specialLTE3_QiD >= 2 & baseM_QiD >= 0.4 & baseM_QiD < 0.6) | (specialLTE3_QiD < 2 & baseM_QiD >= 0.6)]


#--YangD
baseM_YangD <- selectCol(DT_filtered, base_YangD) %>% rowMeans() / 5
specialLTE3_YangD <- selectCol(DT_LTE3, special_YangD) %>% rowSums()

DT_filtered[, b_YangD := specialLTE3_YangD >= 2 & baseM_YangD >= 0.6]
DT_filtered[, t_YangD := (specialLTE3_YangD >= 2 & baseM_YangD >= 0.4 & baseM_YangD < 0.6) | (specialLTE3_YangD < 2 & baseM_YangD >= 0.6)]


#--YinD
baseM_YinD <- selectCol(DT_filtered, base_YinD) %>% rowMeans() / 5
specialLTE3_YinD <- selectCol(DT_LTE3, special_YinD) %>% rowSums()

DT_filtered[, b_YinD := specialLTE3_YinD == 2 & baseM_YinD >= 0.6]
DT_filtered[, t_YinD := (specialLTE3_YinD == 2 & baseM_YinD >= 0.4 & baseM_YinD < 0.6) | (specialLTE3_YinD < 2 & baseM_YinD >= 0.6)]


#--Tan
baseM_Tan <- selectCol(DT_filtered, base_Tan) %>% rowMeans() / 5
specialLTE3_Tan <- selectCol(DT_LTE3, special_Tan) %>% rowSums()
specialLTE4_Tan <- selectCol(DT_LTE4, special_Tan) %>% rowSums()

DT_filtered[, b_Tan := specialLTE4_Tan >= 1 & baseM_Tan >= 0.6]
DT_filtered[, t_Tan := (specialLTE3_Tan >= 1 & baseM_Tan >= 0.4 & baseM_Tan < 0.6) | (specialLTE4_Tan < 1 & baseM_Tan >= 0.6)]


#--Shi
baseM_Shi <- selectCol(DT_filtered, base_Shi) %>% rowMeans() / 5
specialLTE3_Shi <- selectCol(DT_LTE3, special_Shi) %>% rowSums()
specialLTE4_Shi <- selectCol(DT_LTE4, special_Shi) %>% rowSums()

DT_filtered[, b_Shi := specialLTE4_Shi >= 1 & baseM_Shi >= 0.6]
DT_filtered[, t_Shi := (specialLTE3_Shi >= 1 & baseM_Shi >= 0.4 & baseM_Shi < 0.6) | (specialLTE4_Shi < 1 & baseM_Shi >= 0.6)]


#--Xie
baseM_Xie <- selectCol(DT_filtered, base_Xie) %>% rowMeans() / 5
specialLTE3_Xie <- selectCol(DT_LTE3, special_Xie) %>% rowSums()

DT_filtered[, b_Xie := specialLTE3_Xie >= 2 & baseM_Xie >= 0.6]
DT_filtered[, t_Xie := (specialLTE3_Xie >= 2 & baseM_Xie >= 0.4 & baseM_Xie < 0.6) | (specialLTE3_Xie < 2 & baseM_Xie >= 0.6)]


#--QiY
baseM_QiY <- selectCol(DT_filtered, base_QiY) %>% rowMeans() / 5
specialLTE3_QiY <- selectCol(DT_LTE3, special_QiY) %>% rowSums()

DT_filtered[, b_QiY := specialLTE3_QiY >= 2 & baseM_QiY >= 0.6]
DT_filtered[, t_QiY := (specialLTE3_QiY >= 2 & baseM_QiY >= 0.4 & baseM_QiY < 0.6) | (specialLTE3_QiY < 2 & baseM_QiY >= 0.6)]


#--Te
baseM_Te <- selectCol(DT_filtered, base_Te) %>% rowMeans() / 5
specialLTE3_Te <- selectCol(DT_LTE3, special_Te) %>% rowSums()

DT_filtered[, b_Te := specialLTE3_Te == 1 & baseM_Te >= 0.6]
DT_filtered[, t_Te := (specialLTE3_Te == 1 & baseM_Te >= 0.4 & baseM_Te < 0.6) | (specialLTE3_Te < 1 & baseM_Te >= 0.6)]


#--Ping
baseM_Ping <- selectCol(DT_filtered, base_Ping) %>% rowMeans() / 5
specialLTE3_Ping <- selectCol(DT_LTE3, base_Ping) %>% rowSums()
specialLTE3_Ping7 <- (cbind(baseM_QiD, baseM_YangD, baseM_YinD, baseM_Tan, baseM_Shi, baseM_Xie, baseM_QiY) < 0.4) %>% rowSums()

DT_filtered[, b_Ping := specialLTE3_Ping == 7 & baseM_Ping >= 0.7 & specialLTE3_Ping7 >= 5 & baseM_Te < 0.8]
DT_filtered[, bs_JiPing := select(DT_filtered, starts_with("b_"), starts_with("t_")) %>% rowSums() == 0]




"
------------------------------------------------------------
Tabulate
------------------------------------------------------------
"
#--B=1
DT_B1 <- DT_filtered[select(DT_filtered, starts_with("b_")) %>% rowSums() == 1]
colSums(select(DT_B1, starts_with("b_")))


#--B=1, T=0
DT_B1T0 <- DT_filtered[(select(DT_filtered, starts_with("b_")) %>% rowSums() == 1) & (select(DT_filtered, starts_with("t_")) %>% rowSums() == 0)]
colSums(select(DT_B1T0, starts_with("b_")))


#--B=1, T>0
DT_B1TLT0 <- DT_filtered[(select(DT_filtered, starts_with("b_")) %>% rowSums() == 1) & (select(DT_filtered, starts_with("t_")) %>% rowSums() > 0)]
colSums(select(DT_B1TLT0, starts_with("b_")))


#--B=0, T=1
DT_B0T1 <- DT_filtered[(select(DT_filtered, starts_with("b_")) %>% rowSums() == 0) & (select(DT_filtered, starts_with("t_")) %>% rowSums() == 1)]
colSums(select(DT_B0T1, starts_with("t_")))


#--B=0, T>0
DT_B0TLT0 <- DT_filtered[(select(DT_filtered, starts_with("b_")) %>% rowSums() == 0) & (select(DT_filtered, starts_with("t_")) %>% rowSums() > 0)]
colSums(select(DT_B0TLT0, starts_with("t_")))
nrow(DT_B0TLT0)


#--B=0, T>1
DT_B0TLT1 <- DT_filtered[(select(DT_filtered, starts_with("b_")) %>% rowSums() == 0) & (select(DT_filtered, starts_with("t_")) %>% rowSums() > 1)]
colSums(select(DT_B0TLT1, starts_with("t_")))
nrow(DT_B0TLT1)


#--B>0
DT_BLT0 <- DT_filtered[select(DT_filtered, starts_with("b_")) %>% rowSums() > 0]
colSums(select(DT_BLT0, starts_with("b_")))
nrow(DT_BLT0)