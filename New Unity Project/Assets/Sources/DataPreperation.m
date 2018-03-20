clear all; close all;
load('RawDataPerStep.mat');

deleteIndex = find(RawDataPerStep(:,end) == 0);
RawDataPerStep(deleteIndex,:) = [];
[row, col] = size(RawDataPerStep);
shuffledOrder = randperm(row);
shuffledRawData = RawDataPerStep(shuffledOrder, :);

%%
Train = shuffledRawData(:, 1:(end - 2));
Target = zeros(row, 225);
for i = 1:row
    index = shuffledRawData(i, end);
    Target(i, index) = 1;
end