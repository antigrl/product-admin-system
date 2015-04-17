Product Administration System

 

 

 

Software Requirements Specification

 

1.0

 

November 11, 2014

Table of Contents

[[TOC]]

# **1. Introduction**

The introduction to the Software Requirement Specification (SRS) document should provide an overview of the complete SRS document.  

## 1.1 Purpose

*To create a multifunctional database that we can utilize to improve efficiencies and time allocation over multiple departments within GatewayCDI.*

The purpose of this document is to present a detailed description of the **Product Administration System (PAS).** It will explain the purpose and features of the system, the interfaces of the system, what the system will do, the constraints under which it must operate and how the system will react to external stimuli. This document is intended for both the stakeholders and the developers of the system and will be proposed to Senior Management for its approval.

## 1.2 Scope

This **Product Administration System (PAS)** will be a Merchandising and Fulfillment System for the Program Team (PT) of GatewayCDI. PAS will be designed to maximise the PT’s productivity by providing tools to assist in maintaining Client Merchandising and Fulfillment process, which would otherwise require duplicate data entry and processing. By maximizing the PT’s work efficiency and production, PAS will meet the PT’s needs while remaining easy to understand and use.

PAS is designed to allow the PT members to manage merchandising and fulfillment of client products via Campaigns. PAS will facilitate communication between PT members via emails and notifications.

*(1)  Identify the software product(s) to be produced by name*

* *OLD NPR *

* *Client Pricing Spreadsheets*

* *Presentation Sheets*

* *Photo Allowances*

* *Photo List*

* *Product Safety Information*

*(2)  Explain what the software product(s) will, and, if necessary, will not do*

* *PAS will house basic client, campaign and product information. *

* *It will be able to provide reporting based on data that it is storing.*

* *It will replace and store photo allowance data.*

* *It will send emails and notifications of data updates.*

* *It will calculate product sell prices (and margins).*

* *It will create printable presentation sheets for client use.*

* *It will store product safety data.*

* *It will store file and images (logos, product images, documents).*

* *It will replace and store photo list data.*

* *It will have user roles and restricted access.*

* *It will have a changelog with timestamps.*

* *It will be able to copy product data for duplication.*

* *It will store vendor product information.*

* *It will be able to send emails and notifications/reminders based on dates.*

* *It will be able to search and filter all data.*

* *It will be able to track the style/kit header EDP only.*

* *It will be able to house attachments.*

* *It will be able to distinguish between types of attachments (quotes, virtual proofs, screen shots, and testing)*

* *It will have the ability to remove and update attachments*

* *It will have the ability to house dated information and expiration dates based on attachments*

* *It will have a download function to pull attachments back out*

* *It will be able to track vendor data ( contractual photo allowance agreements[‘allowence per product’, ‘total allowance’], )*

* *It will be able to track vendor specific data within NPR’s (GatewayCDI sku, Vendor Sku, quote #, purchase order #, catalog page #, web address, new or carry forward, web or print/ both, total purchase dollars for the item), Paid amount, billed amount, manual open/close status*

* *It will be able to track spring, fall and YTD photo allowance summaries*

* *It will create a pdf invoice for a specific photo allowance amount for a specific vendor*

* *It will create a pdf invoice with vendor specific items and their sku #’s, our sku #’s, our client program, weblink,  catalog page number, and product name*

* * It will have the ability to ‘merge’ multiple NPR’s from multiple campaigns into a ‘carry forward campaign’*

* *It will have a new landing page for the separate applications within PAS*

* *It will not be accessible outside of the GCDI network.*

* *It will not contact clients.*

* *It will not order products.*

* *It will not transfer data into Direct Commerce.*

* *It will not harvest data from external sources.*

* *It will not have an API.*

* *It will not have real time sales data.*

* *It will not be able to track style or kit items.*

* *It will not auto-generate individual NPR’s within a campaign*

*(3)  Describe the application of the software being specified:*

1. *Benefits:*

    1. *Eliminate current duplication of work.*

    2. *Transparency of workflow, communication, and data.*

    3. *Standardization of processes and procedures across all Clients.*

2. *Goals: *

    4. *Reliability, Maintainability, Expandability, & Accuracy. *

    5. *Decrease necessary time allocation to the process.*

    6. *Increase tracking and communication abilities.*

3. *Objectives:*

    7. *Reduce PT’s duplicated work and unify Process*

## 1.3 Glossary

**_PAS_*** - Product Administration System *

*Houses specific information related to the addition and approval of new products into our programs. This portion will be used for basic data entry to cover the necessary information required for transition in to our fulfillment software. This section will also house basic pricing information, product safety information, images, copy, and decoration information.*

**_Photo Allowance_*** - A rebate paid to GatewayCDI from a supplier for advertising their products on our program websites or in physical catalogs.*

**_Photo List_*** - A Google Doc that records tracking information related to the post item selection process.*

**_PT_*** - Program Team*

*	Merchandisers, fulfillment, account managers, and mentors.*

**_DC_*** - Direct Commerce*

*	GatewayCDI’s external proprietary fulfillment software.*

**_IOQ_*** - Initial Order Quantity*

*	The first amount of stock that will be ordered for a given product.*

**_MOQ_*** - Minimum Order Quantity*

*	The smallest amount of stock offered for sale from a supplier of a given product.*

*ALSO: VM - Vendor Minimum*

**_ASP_*** - Annual Sales Projection*

*	An estimation of quantity of stock sold within a year of a given product.*

**_GMO_*** - GatewayCDI Minimum Order*

*	The smallest amount of stock that GatewayCDI will order to maintain profitability for a given product. This can differ from MOQ.*

## 1.4 Reference

*Link to documentation*

*This subsection should:*

*(1)  Provide a complete list of all documents referenced elsewhere in the SRS, or in a separate, specified document.*

*(2)  Identify each document by title, report number - if applicable - date, and publishing organization.*

*(3)  Specify the sources from which the references can be obtained.*

*This information may be provided by reference to an appendix or to another document.*

## 1.5 Overview

*This subsection should:*

*(1) Describe what the rest of the SRS contains*

*(2) Explain how the SRS is organized.*

# **2. General Description**

*This section of the SRS should describe the general factors that affect the product and its requirements.  It should be made clear that this section does not state specific requirements; it only makes those requirements easier to understand.*

* *PAS will house basic client, campaign and product information. *

* *It will be able to provide reporting based on data that it is storing.*

* *It will replace and store photo allowance data.*

* *It will send emails and notifications of data updates.*

* *It will calculate product sell prices (and margins).*

* *It will create printable presentation sheets for client use.*

* *It will store product safety data.*

* *It will store file and images (logos, product images, documents).*

* *It will replace and store photo list data.*

* *It will have user roles and restricted access.*

* *It will have a changelog with timestamps.*

* *It will be able to copy product data for duplication.*

* *It will store vendor product information.*

* *It will be able to send emails and notifications/reminders based on dates.*

* *It will be able to search and filter all data.*

* *It will be able to track the style/kit header EDP only.*

## 2.1 Product Perspective

*This subsection of the SRS puts the product into perspective with other related products or projects.  (See the IEEE Guide to SRS for more details).*

## 2.2 Product Functions

This subsection of the SRS should provide a summary of the functions that the software will perform.

## 2.3 User Characteristics

This subsection of the SRS should describe those general characteristics of the eventual users of the product that will affect the specific requirements.  (See the IEEE Guide to SRS for more details).

## 2.4 General Constraints

*This subsection of the SRS should provide a general description of any other items that will*

*limit the developer’s options for designing the system. (See the IEEE Guide to SRS for a partial list of possible general constraints).*

## 2.5 Assumptions and Dependencies

This subsection of the SRS should list each of the factors that affect the requirements stated in the SRS. These factors are not design constraints on the software but are, rather, any changes to them that can affect the requirements in the SRS. For example, an assumption might be that a specific operating system will be available on the hardware designated for the software product. If, in fact, the operating system is not available, the SRS would then have to change accordingly.

# **3. Specific Requirements**

This will be the largest and most important section of the SRS.  The customer requirements will be embodied within Section 2, but this section will give the D-requirements that are used to guide the project’s software design, implementation, and testing.

 

Each requirement in this section should be:

·         Correct

·         Traceable (both forward and backward to prior/future artifacts)

·         Unambiguous

·         Verifiable (i.e., testable)

·         Prioritized (with respect to importance and/or stability)

·         Complete

·         Consistent

·         Uniquely identifiable (usually via numbering like 3.4.5.6)

 

Attention should be paid to carefully organize the requirements presented in this section so that they may be easily accessed and understood. Furthermore, this SRS is not the software design document, therefore one should avoid the tendency to over-constrain (and therefore design) the software project within this SRS.

## 3.1 External Interface Requirements

### 3.1.1 User Interfaces

### 3.1.2 Hardware Interfaces

### 3.1.3 Software Interfaces

### 3.1.4 Communications Interfaces

## 3.2 Functional Requirements

This section describes specific features of the software project.  If desired, some requirements may be specified in the use-case format and listed in the Use Cases Section.

### 3.2.1 Phase 1 - Presentation Sheets

* Item Description size will be limited to a number of max characters.. 

* Item Description needs to accommodate for symbols, bullet points, etc.

* Country of Origin should begin with "Made in " and then pull the country from the PAS.

* Minimum Order Quantity box should begin with "MOQ: " and pull information from the Vendor Minimum box in the PAS.

* Pricing Tiers should be set to pull the current maximum of three tiers stacked vertically in the box (see diagram). 

* We should have an interface/summary page where we can select 1,2, or 3 possible tiers to show/hide.

* Icons should appear on the interface/summary page with an option to show the icons.

    * [Phase 1]

        * input ‘tags’ within the description(savable)

    * [Phase 2]

        * Have checkboxes within product edit for ‘tags’

* Icons should be static and always remain in the same position.

* Product Major Category listed at top of Page.  Label G in diagram.  This is pulled directly from the PAS.

* Ability to Print by a selected Major Category or all categories added to interface. 

* Item Sort functionality based on Major Category, and then secondary sort by Minor Category Name.

* Option to change where the items are ranked for printing purposes. [drag and drop]

* Each Page will have a Page Number.  "Page 1", "Page 6", etc.

    * with option to input a starting page number

3.2.1.1 Introduction

3.2.1.2 Inputs

3.2.1.3 Processing

3.2.1.4 Outputs

3.2.1.5 Error Handling

### 3.2.2 Phase 2 - <>

## 3.3 Use Cases & User Stories

## 3.3.1 Create new Campaign

## Use Case

<table>
  <tr>
    <td>Name</td>
    <td>Create New Campaign</td>
  </tr>
  <tr>
    <td>Summary</td>
    <td></td>
  </tr>
  <tr>
    <td>Rationale</td>
    <td></td>
  </tr>
  <tr>
    <td>Users</td>
    <td></td>
  </tr>
  <tr>
    <td>Preconditions</td>
    <td></td>
  </tr>
  <tr>
    <td>Basic Course of Events</td>
    <td></td>
  </tr>
  <tr>
    <td>Alternative Paths</td>
    <td></td>
  </tr>
  <tr>
    <td>Postconditions</td>
    <td></td>
  </tr>
</table>


**User Story**

Create a new Campaign

## 3.3.2 Presentation Sheets

## 3.3.3 Quote Forms 

* Be able to upload vendor quote form. 

## 3.4 Classes / Objects

### 3.4.1 John’s class library

 

3.4.1.1 Attributes

3.4.1.2 Functions

<Reference to functional requirements and/or use cases>

### 3.4.2 <Class / Object #2>

…

## 3.5 Non-Functional Requirements

Non-functional requirements may exist for the following attributes. Often these requirements must be achieved at a system-wide level rather than at a unit level. State the requirements in the following sections in measurable terms (e.g., 95% of transaction shall be processed in less than a second, system downtime may not exceed 1 minute per day, > 30 day MTBF value, etc).

### 3.5.1 Performance

### 3.5.2 Reliability

### 3.5.3 Availability

### 3.5.4 Security

### 3.5.5 Maintainability

### 3.5.6 Portability

## 3.6 Inverse Requirements

State any *useful* inverse requirements.

## 3.7 Design Constraints

Specify design constraints imposed by other standards, company policies, hardware limitation, etc. that will impact this software project.

## 3.8 Logical Database Requirements

Will a database be used?  If so, what logical requirements exist for data formats, storage capabilities, data retention, data integrity, etc.

## 3.9 Other Requirements

Catchall section for any additional requirements.

# **4. Analysis Models**

List all analysis models used in developing specific requirements previously given in this SRS.  Each model should include an introduction and a narrative description.  Furthermore, each model should be traceable in the SRS’s requirements.

## 4.1 Sequence Diagrams

## 4.3 Data Flow Diagrams (DFD)

## 4.2 State-Transition Diagrams (STD)

# **5. Change Management Process**

Identify and describe the process that will be used to update the SRS, as needed, when project scope or requirements change.  Who can submit changes and by what means, and how will these changes be approved.

# **A. Appendices**

Appendices may be used to provide additional (and hopefully helpful) information. If present, the SRS should explicitly state whether the information contained within an appendix is to be considered as a part of the SRS’s overall set of requirements.

* *

*Example Appendices could include (initial) conceptual documents for the software project, marketing materials, minutes of meetings with the customer(s), etc.*

## A.1 Appendix 1

## A.2 Appendix 2

