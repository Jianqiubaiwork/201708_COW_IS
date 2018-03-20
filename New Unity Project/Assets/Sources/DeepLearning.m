clear all; close all;
load('TargetWithAI3.mat');  
load('TrainWithAI3.mat');
%load('deepnet.mat');

%%
[row, col] = size(TrainWithAI3);
devidedRow = round(row * 0.8);

TTrain = TrainWithAI3(:,1:end);
xTrain = TTrain(1:devidedRow, :);
xTest = TTrain((devidedRow + 1):end, :);

TTarget = TargetWithAI3;
tTrain = TTarget(1:devidedRow, :);
tTest = TTarget((devidedRow + 1):end, :);

rng('default') %randomly initialized weights
%%
hiddenSize1 = 100;

autoenc1 = trainAutoencoder(xTrain',hiddenSize1, ...
    'MaxEpochs',400, ...
    'L2WeightRegularization',0.004, ...
    'SparsityRegularization',4, ...
    'SparsityProportion',0.15, ...
    'ScaleData', false);

feat1 = encode(autoenc1,xTrain')';

%%
hiddenSize2 = 50;
autoenc2 = trainAutoencoder(feat1',hiddenSize2, ...
    'MaxEpochs',100, ...
    'L2WeightRegularization',0.002, ...
    'SparsityRegularization',4, ...
    'SparsityProportion',0.1, ...
    'ScaleData', false);

feat2 = encode(autoenc2,feat1')';

%%
% hiddenSize3 = 100;
% autoenc3 = trainAutoencoder(feat2,hiddenSize3, ...
%     'MaxEpochs',200, ...
%     'L2WeightRegularization',0.002, ...
%     'SparsityRegularization',2, ...
%     'SparsityProportion',0.1, ...
%     'ScaleData', false);
% 
% feat3 = encode(autoenc3,feat2);

%%
% hiddenSize4 = 50;
% autoenc4 = trainAutoencoder(feat3,hiddenSize4, ...
%     'MaxEpochs',100, ...
%     'L2WeightRegularization',0.001, ...
%     'SparsityRegularization',1, ...
%     'SparsityProportion',0.1, ...
%     'ScaleData', false);
% 
% feat4 = encode(autoenc4,feat3);

%%
softnet = trainSoftmaxLayer(feat2',tTrain','MaxEpochs',400);

deepnet = stack(autoenc1,autoenc2, softnet);

view(deepnet)

%%
view(autoenc1)

%%
yTest = deepnet(xTest')';
[yV, yI] = max(yTest');
yI = yI';
[testNum, yCol] = size(yTest);
yyTest = zeros(testNum, yCol);
for i = 1:testNum
    yyTest(i, yI(i)) = 1;
end

%%
compare = zeros(testNum, 2);
correct = 0;
oneTileAway = 0;
twoTileAway = 0;
threeTileAway = 0;
others = 0;
for i = 1:testNum
    compare(i,1) = find(yyTest(i,:)); % output
    compare(i,2) = find(tTest(i,:)); % target values
    diff = abs(compare(i,1) - compare(i,2));
    if (diff == 0)
        correct = correct + 1;
    elseif (diff == 1 || diff == 15)
        oneTileAway = oneTileAway + 1;
    elseif (diff == 2 || diff == 30)
        twoTileAway = twoTileAway + 1;
    elseif (diff == 3 || diff == 45)
        threeTileAway = threeTileAway + 1;
    else
        others = others + 1;
    end
end
error = correct/testNum;
%fname = {'0','1','2','3','other'};
%name = [fname];
barY = [correct; oneTileAway; twoTileAway; threeTileAway; others];
%set(gca,'xticktlabel',name);
bar(barY);

%%
A = zeros(225, 1);
%%
step = 'Step 1 !!!'
myx = 7;
myy = 7;
A(15*myy + myx + 1) = 2;
DisplayA = zeros(15, 15);
for row = 1:15
    for col = 1:15
        x = col;
        y = 15 - row;
        DisplayA(row, col) = A(15*y + x);
    end
end
B = deepnet(A);
[BV, BI] = max(B);
DisplayA
BI = BI - 1

%%
step = 'Step 2 !!!'
opx = mod(BI, 15);
opy = floor(BI/15);
%opx = 6;
%opy = 7;
A(15*opy + opx + 1) = 1;
for row = 1:15
    for col = 1:15
        x = col;
        y = 15 - row;
        DisplayA(row, col) = A(15*y + x);
    end
end
DisplayA
%%
step = 'Step 3 !!!'
myx = 7;
myy = 6;
A(15*myy + myx + 1) = 2;
DisplayA = zeros(15, 15);
for row = 1:15
    for col = 1:15
        x = col;
        y = 15 - row;
        DisplayA(row, col) = A(15*y + x);
    end
end
B = deepnet(A);
[BV, BI] = max(B);
DisplayA
BI = BI - 1

%%
step = 'Another Step 3 !!!'
B(BI + 1) = 0;
[BV, BI] = max(B);

BI = BI - 1
%%
step = 'Step 4 !!!'
opx = mod(BI, 15);
opy = floor(BI/15);
%opx = 6;
%opy = 7;
A(15*opy + opx + 1) = 1;
for row = 1:15
    for col = 1:15
        x = col;
        y = 15 - row;
        DisplayA(row, col) = A(15*y + x);
    end
end
DisplayA
%%
step = 'Step 5 !!!'
myx = 7;
myy = 8;
A(15*myy + myx + 1) = 2;
DisplayA = zeros(15, 15);
for row = 1:15
    for col = 1:15
        x = col;
        y = 15 - row;
        DisplayA(row, col) = A(15*y + x);
    end
end
B = deepnet(A);
[BV, BI] = max(B);
DisplayA
BI = BI - 1

%%
step = 'Another Step 5 !!!'
B(BI + 1) = 0;
[BV, BI] = max(B);

BI = BI - 1

%%
step = 'Step 6 !!!'
opx = mod(BI, 15);
opy = floor(BI/15);
%opx = 6;
%opy = 7;
A(15*opy + opx + 1) = 1;
for row = 1:15
    for col = 1:15
        x = col;
        y = 15 - row;
        DisplayA(row, col) = A(15*y + x);
    end
end
DisplayA
%%
step = 'Step 7 !!!'
myx = 4;
myy = 6;
A(15*myy + myx + 1) = 2;
DisplayA = zeros(15, 15);
for row = 1:15
    for col = 1:15
        x = col;
        y = 15 - row;
        DisplayA(row, col) = A(15*y + x);
    end
end
B = deepnet(A);
[BV, BI] = max(B);
DisplayA
BI = BI - 1

%%
step = 'Another Step 7 !!!'
B(BI + 1) = 0;
[BV, BI] = max(B);

BI = BI - 1

%%
step = 'Step 8 !!!'
opx = mod(BI, 15);
opy = floor(BI/15);
%opx = 6;
%opy = 7;
A(15*opy + opx + 1) = 1;
for row = 1:15
    for col = 1:15
        x = col;
        y = 15 - row;
        DisplayA(row, col) = A(15*y + x);
    end
end
DisplayA