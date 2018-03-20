clear all; close all;

%% Greedy vs. Minimax
load('GreedyVsMinimax')
n = length(GreedyVsMinimax);
Greedy = 0;
Mini = 0;
for i = 1:2:n
    if (GreedyVsMinimax(i, 226) == 1)
        Greedy = Greedy + 1;
    else
        Mini = Mini + 1;
    end    
end

for i = 2:2:n
    if (GreedyVsMinimax(i, 226) == 2)
        Greedy = Greedy + 1;
    else
        Mini = Mini + 1;
    end    
end

%% Greedy vs. ANN
load('Experiments2')
n = length(Experiments2);
Greedy = 0;
ANN = 0;
for i = 1:2:n
    if (Experiments2(i, 226) == 1)
        Greedy = Greedy + 1;
    else
        ANN = ANN + 1;
    end    
end

for i = 2:2:n
    if (Experiments2(i, 226) == 2)
        Greedy = Greedy + 1;
    else
        ANN = ANN + 1;
    end    
end

%% Minimax vs. ANN
load('Experiments3')
n = length(Experiments3);
Mini = 0;
ANN = 0;
for i = 1:2:n
    if (Experiments3(i, 226) == 1)
        Mini = Mini + 1;
    else
        ANN = ANN + 1;
    end    
end

for i = 2:2:n
    if (Experiments3(i, 226) == 2)
        Mini = Mini + 1;
    else
        ANN = ANN + 1;
    end    
end
