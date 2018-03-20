clear all; close all;
load('W3.mat');

[row, col] = size(W3);
fileID = fopen('W3.txt', 'w');
for i = 1:row
    for j = 1:col
        fprintf(fileID, '%.15f\n', W3(i,j));
    end
end