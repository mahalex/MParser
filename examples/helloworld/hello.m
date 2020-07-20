x = 2;
f(x);
x = 5;
f(x);
x = 3;
f(x);

% for i = 1:10
%     disp(i);
% end


function f(x)
    disp('X was');
    disp(x);
    if x > 3
        disp('greater than 3!');
    elseif x < 3
        disp('less than 3!');
    else
        disp('exactly 3!');
    end
    x = x + 1;
    disp('X + 1 is');
    disp(x);
end