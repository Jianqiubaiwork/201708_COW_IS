clear all; close all;
load('W2.mat');

[row, col] = size(W2);
fileID = fopen('W2.txt', 'w');
for i = 1:row
    for j = 1:col
        fprintf(fileID, '%.15f\n', W2(i,j));
    end
end