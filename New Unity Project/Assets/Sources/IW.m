clear all; close all;
load('IW.mat');

[row, col] = size(IW);
fileID = fopen('IW.txt', 'w');
for i = 1:row
    for j = 1:col
        fprintf(fileID, '%.15f\n', IW(i, j));
    end
end