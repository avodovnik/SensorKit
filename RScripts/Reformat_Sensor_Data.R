datafile <- "C:\\Projects\\SensorReport\\ski_sensor_long_anonymized.csv"
dataset1 <- read.csv(datafile, sep=",", header=TRUE)

parameter_file <- "C:\\Projects\\SensorReport\\Reformat_Params.csv"
dataset2 <- read.csv(parameter_file, sep="\t", header=F, quote="'")

library(rjson)
library(stringr)
# dataset2 will be read in as rows, if the original json text is in multiple rows. 
# Therefore, we need to concatenate these multiple rows into 
# a single string with the right json structure
parameters_json <- paste(dataset2[['V1']], collapse='')
parameters <- fromJSON(parameters_json)

extract_columns <- function(inputstring, sep=','){
  outputArray <- strsplit(inputstring, sep)[[1]]
  outputArray <- gsub("^\\s+|\\s+$", "", outputArray)
  return(outputArray)
}

Feature_Columns <- extract_columns(parameters$Features)
Id_Columns <- extract_columns(parameters$Ids)
Dcast_Columns <- extract_columns(parameters$Dcast_Columns)

Sys.setlocale("LC_TIME", "C")
Sys.setlocale("LC_COLLATE", "C") 
Sys.setlocale("LC_TIME", "English")

##install libraries
list.of.packages <- c('base64enc', 'dplyr', 'reshape2', 'ggplot2', 'zoo')
new.packages <- list.of.packages[!(list.of.packages %in% installed.packages()[,'Package'])]
if(length(new.packages))
  install.packages(new.packages, dependencies = TRUE)

library(base64enc)
library(dplyr)
library(reshape2) # http://seananderson.ca/2013/10/19/reshape.html
library(ggplot2)
library(zoo)

dataset1$index<-rownames(dataset1)

dataset1 <- dataset1[Feature_Columns]

melted <- melt(dataset1, id.vars = Id_Columns)
melted$variable<-paste(melted$tag, melted$variable)
melted$tag<-NULL 

execute_string <- paste('widedata <- dcast(melted, ', paste(Dcast_Columns, collapse = ' + '), ' ~ variable)', sep='')
eval(parse(text = execute_string))

# unify the column names by removing spaces in variable names, and use lower cases for all letters
cn <- colnames(widedata)
cn <- gsub(" ", "_", cn)
cn <- gsub("([a-z])([A-Z])", "\\1_\\L\\2", cn, perl = TRUE)
cn <- sub("^(.[a-z])", "\\L\\1", cn, perl = TRUE) # make 1st char lower case

colnames(widedata) <- cn

# Write the wide data to a local csv file for further process and analysis
outputfile <- "C:\\Projects\\SensorReport\\wide_sensor_data_anonymized.csv"
write.csv(widedata, file=outputfile, sep=",", row.names=FALSE, quote=FALSE)