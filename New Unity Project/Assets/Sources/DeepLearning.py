########## Import Packages ##########
import numpy as np
import pandas as pd
from sklearn.neural_network import MLPClassifier

########## Data Preperation ##########
#RawDataWithAI = pd.read_excel('RawDataWithAI.xlsx')
#RawData = RawData.dropna()
#RawData = RawData.astype(int)
#RawMatrix = RawDataWithAI.values

#RawData = pd.read_excel('Train.xlsx')
#RawData = RawData.dropna()
#RawData = RawData.astype(int)
#MatrixData = RawData.values

#Target = pd.read_excel('Target.xlsx')
#Target = Target.dropna()
#Target = Target.astype(int)
#MatrixTarget = Target.values

print (1)
#row = MatrixData.shape[0] - 1
#col1 = MatrixData.shape[1] - 1
#col2 = MatrixTarget.shape[1] - 1
#DivideRow = int(row*0.8)

#TrainData = MatrixData[0:DivideRow, 0:col1]
#TrainTarget = MatrixTarget[0:DivideRow, 0:col2]
#TestData = MatrixData[(DivideRow+1):row, 0:col1]
#TestTarget = MatrixData[(DivideRow+1):row, 0:col2]

########## Data Training ##########
#mlp = MLPClassifier(hidden_layer_sizes=(50,), max_iter=200, alpha=1e-2,
#                    solver='sgd', verbose=10, tol=1e-1, random_state=1,
#                    learning_rate_init=.1)

#mlp.fit(TrainData, TrainTarget)
#print(mlp.predict(TestData))
#print(TestTarget)