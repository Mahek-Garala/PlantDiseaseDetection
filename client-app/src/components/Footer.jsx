import React from "react";

const Footer = () => {
    return (
        <footer className="bg-gray-200 text-center py-4 text-gray-700 text-sm">
            &copy; {new Date().getFullYear()} FarmEase. All Rights Reserved.
        </footer>
    );
};

export default Footer;
