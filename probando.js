function calculateDaysBetweenDates(begin, end) {
    // Validate input
    if (begin === null || end === null)
        return 0;

    // Convert to date object
    begin = new Date(begin);
    end = new Date(end);

    // Validate input
    if (begin === "Invalid Date" || end === "Invalid Date")
        return 0;

    // Calculate days between dates
    var days = Math.floor((end - begin) / (1000 * 60 * 60 * 24));

    // Return difference
    return days;
}